using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LeaveFlow.LeaveRequests;

public interface ILeaveRequestRepository : IRepository<LeaveRequest, Guid>
{
    Task<List<LeaveRequest>> GetListByRequesterIdAsync(
        Guid requesterId,
        CancellationToken cancellationToken = default);
    
    Task<List<LeaveRequest>> GetPendingRequestsForApproverAsync(
        CancellationToken cancellationToken = default);
    
    Task<List<LeaveRequest>> GetOverlappingRequestsAsync(
        Guid requesterId,
        DateTime startDate,
        DateTime endDate,
        Guid? excludeRequestId = null,
        CancellationToken cancellationToken = default);
}
