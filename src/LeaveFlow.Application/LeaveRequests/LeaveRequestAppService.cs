using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LeaveFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace LeaveFlow.LeaveRequests;

[Authorize(LeaveFlowPermissions.LeaveRequests.Default)]
public class LeaveRequestAppService : LeaveFlowAppService, ILeaveRequestAppService
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly LeaveRequestManager _leaveRequestManager;
    private readonly IIdentityUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public LeaveRequestAppService(
        ILeaveRequestRepository leaveRequestRepository,
        LeaveRequestManager leaveRequestManager,
        IIdentityUserRepository userRepository,
        ICurrentUser currentUser)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _leaveRequestManager = leaveRequestManager;
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<LeaveRequestDto> GetAsync(Guid id)
    {
        var leaveRequest = await _leaveRequestRepository.GetAsync(id);
        return await MapToDtoAsync(leaveRequest);
    }

    public async Task<PagedResultDto<LeaveRequestDto>> GetListAsync(GetLeaveRequestListDto input)
    {
        var queryable = await _leaveRequestRepository.GetQueryableAsync();

        queryable = ApplyFilters(queryable, input);

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable
            .OrderByDescending(x => x.CreationTime)
            .PageBy(input.SkipCount, input.MaxResultCount);

        var leaveRequests = await AsyncExecuter.ToListAsync(queryable);

        var dtos = new List<LeaveRequestDto>();
        foreach (var request in leaveRequests)
        {
            dtos.Add(await MapToDtoAsync(request));
        }

        return new PagedResultDto<LeaveRequestDto>(totalCount, dtos);
    }

    [Authorize(LeaveFlowPermissions.LeaveRequests.Create)]
    public async Task<LeaveRequestDto> CreateAsync(CreateLeaveRequestDto input)
    {
        var requesterId = _currentUser.GetId();

        var leaveRequest = await _leaveRequestManager.CreateAsync(
            input.LeaveTypeId,
            requesterId,
            input.StartDate,
            input.EndDate,
            input.Reason
        );

        await _leaveRequestRepository.InsertAsync(leaveRequest, autoSave: true);

        return await MapToDtoAsync(leaveRequest);
    }

    [Authorize(LeaveFlowPermissions.LeaveRequests.Approve)]
    public async Task<LeaveRequestDto> ApproveAsync(Guid id, ApproveLeaveRequestDto input)
    {
        var leaveRequest = await _leaveRequestRepository.GetAsync(id);
        var approverId = _currentUser.GetId();

        // Cannot approve own request
        if (leaveRequest.RequesterId == approverId)
        {
            throw new BusinessException("LeaveFlow:00010");
        }

        await _leaveRequestManager.ApproveAsync(leaveRequest, approverId, input.Notes);
        await _leaveRequestRepository.UpdateAsync(leaveRequest, autoSave: true);

        return await MapToDtoAsync(leaveRequest);
    }

    [Authorize(LeaveFlowPermissions.LeaveRequests.Approve)]
    public async Task<LeaveRequestDto> RejectAsync(Guid id, RejectLeaveRequestDto input)
    {
        var leaveRequest = await _leaveRequestRepository.GetAsync(id);
        var approverId = _currentUser.GetId();

        // Cannot reject own request
        if (leaveRequest.RequesterId == approverId)
        {
            throw new BusinessException("LeaveFlow:00010");
        }

        await _leaveRequestManager.RejectAsync(leaveRequest, approverId, input.Notes);
        await _leaveRequestRepository.UpdateAsync(leaveRequest, autoSave: true);

        return await MapToDtoAsync(leaveRequest);
    }

    public async Task<LeaveRequestDto> CancelAsync(Guid id)
    {
        var leaveRequest = await _leaveRequestRepository.GetAsync(id);

        // Only requester can cancel their own request
        // Or someone with Approve permission can cancel any request
        var currentUserId = _currentUser.GetId();
        if (leaveRequest.RequesterId != currentUserId)
        {
            await AuthorizationService.CheckAsync(LeaveFlowPermissions.LeaveRequests.Approve);
        }

        await _leaveRequestManager.CancelAsync(leaveRequest);
        await _leaveRequestRepository.UpdateAsync(leaveRequest, autoSave: true);

        return await MapToDtoAsync(leaveRequest);
    }

    [Authorize(LeaveFlowPermissions.LeaveRequests.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _leaveRequestRepository.DeleteAsync(id);
    }

    public async Task<PagedResultDto<LeaveRequestDto>> GetMyRequestsAsync(GetLeaveRequestListDto input)
    {
        var currentUserId = _currentUser.GetId();
        input.RequesterId = currentUserId;

        return await GetListAsync(input);
    }

    [Authorize(LeaveFlowPermissions.LeaveRequests.Approve)]
    public async Task<PagedResultDto<LeaveRequestDto>> GetPendingApprovalsAsync(GetLeaveRequestListDto input)
    {
        input.Status = LeaveRequestStatus.Pending;
        return await GetListAsync(input);
    }

    #region Private Methods

    private IQueryable<LeaveRequest> ApplyFilters(IQueryable<LeaveRequest> queryable, GetLeaveRequestListDto input)
    {
        return queryable
            .WhereIf(input.LeaveTypeId.HasValue, x => x.LeaveTypeId == input.LeaveTypeId!.Value)
            .WhereIf(input.RequesterId.HasValue, x => x.RequesterId == input.RequesterId!.Value)
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value)
            .WhereIf(input.StartDateFrom.HasValue, x => x.StartDate >= input.StartDateFrom!.Value)
            .WhereIf(input.StartDateTo.HasValue, x => x.StartDate <= input.StartDateTo!.Value);
    }

    private async Task<LeaveRequestDto> MapToDtoAsync(LeaveRequest leaveRequest)
    {
        var dto = ObjectMapper.Map<LeaveRequest, LeaveRequestDto>(leaveRequest);

        // Get requester name
        var requester = await _userRepository.FindAsync(leaveRequest.RequesterId);
        dto.RequesterName = requester?.Name ?? requester?.UserName ?? "Unknown";

        // Get leave type name
        if (leaveRequest.LeaveType != null)
        {
            dto.LeaveTypeName = leaveRequest.LeaveType.Name;
        }

        // Get approver name if exists
        if (leaveRequest.ApproverId.HasValue)
        {
            var approver = await _userRepository.FindAsync(leaveRequest.ApproverId.Value);
            dto.ApproverName = approver?.Name ?? approver?.UserName;
        }

        dto.TotalDays = leaveRequest.TotalDays;

        return dto;
    }

    #endregion
}
