using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LeaveFlow.LeaveTypes;
using LeaveFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace LeaveFlow.LeaveBalances;

[Authorize(LeaveFlowPermissions.LeaveBalances.Default)]
public class LeaveBalanceAppService : LeaveFlowAppService, ILeaveBalanceAppService
{
    private readonly ILeaveBalanceRepository _leaveBalanceRepository;
    private readonly IRepository<LeaveType, Guid> _leaveTypeRepository;
    private readonly IIdentityUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public LeaveBalanceAppService(
        ILeaveBalanceRepository leaveBalanceRepository,
        IRepository<LeaveType, Guid> leaveTypeRepository,
        IIdentityUserRepository userRepository,
        ICurrentUser currentUser)
    {
        _leaveBalanceRepository = leaveBalanceRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<LeaveBalanceDto> GetAsync(Guid id)
    {
        var balance = await _leaveBalanceRepository.GetAsync(id);
        return await MapToDtoAsync(balance);
    }

    public async Task<PagedResultDto<LeaveBalanceDto>> GetListAsync(GetLeaveBalanceListDto input)
    {
        var queryable = await _leaveBalanceRepository.GetQueryableAsync();

        queryable = queryable
            .WhereIf(input.UserId.HasValue, x => x.UserId == input.UserId!.Value)
            .WhereIf(input.LeaveTypeId.HasValue, x => x.LeaveTypeId == input.LeaveTypeId!.Value)
            .WhereIf(input.Year.HasValue, x => x.Year == input.Year!.Value);

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable
            .OrderBy(x => x.Year)
            .ThenBy(x => x.UserId)
            .PageBy(input.SkipCount, input.MaxResultCount);

        var balances = await AsyncExecuter.ToListAsync(queryable);

        var dtos = new List<LeaveBalanceDto>();
        foreach (var balance in balances)
        {
            dtos.Add(await MapToDtoAsync(balance));
        }

        return new PagedResultDto<LeaveBalanceDto>(totalCount, dtos);
    }

    [Authorize(LeaveFlowPermissions.LeaveBalances.Create)]
    public async Task<LeaveBalanceDto> CreateAsync(CreateUpdateLeaveBalanceDto input)
    {
        // Check if balance already exists
        var existing = await _leaveBalanceRepository.GetByUserAndLeaveTypeAsync(
            input.UserId, input.LeaveTypeId, input.Year);

        if (existing != null)
        {
            throw new BusinessException(LeaveFlowDomainErrorCodes.LeaveBalanceAlreadyExists);
        }

        var balance = new LeaveBalance(
            GuidGenerator.Create(),
            input.UserId,
            input.LeaveTypeId,
            input.Year,
            input.TotalDays
        );

        await _leaveBalanceRepository.InsertAsync(balance, autoSave: true);

        return await MapToDtoAsync(balance);
    }

    [Authorize(LeaveFlowPermissions.LeaveBalances.Edit)]
    public async Task<LeaveBalanceDto> UpdateAsync(Guid id, CreateUpdateLeaveBalanceDto input)
    {
        var balance = await _leaveBalanceRepository.GetAsync(id);
        
        balance.AdjustTotalDays(input.TotalDays);

        await _leaveBalanceRepository.UpdateAsync(balance, autoSave: true);

        return await MapToDtoAsync(balance);
    }

    [Authorize(LeaveFlowPermissions.LeaveBalances.Edit)]
    public async Task DeleteAsync(Guid id)
    {
        await _leaveBalanceRepository.DeleteAsync(id);
    }

    public async Task<List<LeaveBalanceDto>> GetMyBalancesAsync(int? year = null)
    {
        var currentUserId = _currentUser.GetId();
        var targetYear = year ?? DateTime.Now.Year;

        var balances = await _leaveBalanceRepository.GetListByUserIdAsync(currentUserId, targetYear);

        var dtos = new List<LeaveBalanceDto>();
        foreach (var balance in balances)
        {
            dtos.Add(await MapToDtoAsync(balance));
        }

        return dtos;
    }

    [Authorize(LeaveFlowPermissions.LeaveBalances.Create)]
    public async Task InitializeBalancesForUserAsync(Guid userId, int year)
    {
        // Get all active leave types
        var leaveTypesQueryable = await _leaveTypeRepository.GetQueryableAsync();
        var activeLeaveTypes = await AsyncExecuter.ToListAsync(
            leaveTypesQueryable.Where(x => x.IsActive)
        );

        foreach (var leaveType in activeLeaveTypes)
        {
            // Check if balance already exists
            var existing = await _leaveBalanceRepository.GetByUserAndLeaveTypeAsync(
                userId, leaveType.Id, year);

            if (existing == null)
            {
                var balance = new LeaveBalance(
                    GuidGenerator.Create(),
                    userId,
                    leaveType.Id,
                    year,
                    leaveType.DefaultDays
                );

                await _leaveBalanceRepository.InsertAsync(balance);
            }
        }
    }

    #region Private Methods

    private async Task<LeaveBalanceDto> MapToDtoAsync(LeaveBalance balance)
    {
        var dto = ObjectMapper.Map<LeaveBalance, LeaveBalanceDto>(balance);

        // Get user name
        var user = await _userRepository.FindAsync(balance.UserId);
        dto.UserName = user?.Name ?? user?.UserName ?? "Unknown";

        // Get leave type name
        if (balance.LeaveType != null)
        {
            dto.LeaveTypeName = balance.LeaveType.Name;
        }
        else
        {
            var leaveType = await _leaveTypeRepository.FindAsync(balance.LeaveTypeId);
            dto.LeaveTypeName = leaveType?.Name ?? "Unknown";
        }

        dto.RemainingDays = balance.RemainingDays;

        return dto;
    }

    #endregion
}
