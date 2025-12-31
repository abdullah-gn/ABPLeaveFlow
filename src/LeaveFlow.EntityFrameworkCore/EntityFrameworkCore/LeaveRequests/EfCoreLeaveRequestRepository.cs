using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeaveFlow.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeaveFlow.LeaveRequests;

public class EfCoreLeaveRequestRepository 
    : EfCoreRepository<LeaveFlowDbContext, LeaveRequest, Guid>, ILeaveRequestRepository
{
    public EfCoreLeaveRequestRepository(IDbContextProvider<LeaveFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<LeaveRequest>> GetListByRequesterIdAsync(
        Guid requesterId,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        
        return await dbSet
            .Where(x => x.RequesterId == requesterId)
            .Include(x => x.LeaveType)
            .OrderByDescending(x => x.CreationTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LeaveRequest>> GetPendingRequestsForApproverAsync(
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        
        return await dbSet
            .Where(x => x.Status == LeaveRequestStatus.Pending)
            .Include(x => x.LeaveType)
            .OrderBy(x => x.CreationTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LeaveRequest>> GetOverlappingRequestsAsync(
        Guid requesterId,
        DateTime startDate,
        DateTime endDate,
        Guid? excludeRequestId = null,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        
        var query = dbSet
            .Where(x => x.RequesterId == requesterId)
            .Where(x => x.Status != LeaveRequestStatus.Cancelled && x.Status != LeaveRequestStatus.Rejected)
            .Where(x => x.StartDate <= endDate && x.EndDate >= startDate);

        if (excludeRequestId.HasValue)
        {
            query = query.Where(x => x.Id != excludeRequestId.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
}
