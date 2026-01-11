using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeaveFlow.LeaveRequests;

public interface ILeaveRequestAppService : IApplicationService
{
    Task<LeaveRequestDto> GetAsync(Guid id);
    
    Task<PagedResultDto<LeaveRequestDto>> GetListAsync(GetLeaveRequestListDto input);
    
    Task<LeaveRequestDto> CreateAsync(CreateLeaveRequestDto input);
    
    Task<LeaveRequestDto> ApproveAsync(Guid id, ApproveLeaveRequestDto input);
    
    Task<LeaveRequestDto> RejectAsync(Guid id, RejectLeaveRequestDto input);
    
    Task<LeaveRequestDto> CancelAsync(Guid id);
    
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Get leave requests for the current logged-in user
    /// </summary>
    Task<PagedResultDto<LeaveRequestDto>> GetMyRequestsAsync(GetLeaveRequestListDto input);
    
    /// <summary>
    /// Get all pending requests (for approvers/managers)
    /// </summary>
    Task<PagedResultDto<LeaveRequestDto>> GetPendingApprovalsAsync(GetLeaveRequestListDto input);
}
