using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LeaveFlow.LeaveBalances;

public interface ILeaveBalanceRepository : IRepository<LeaveBalance, Guid>
{
    Task<LeaveBalance?> GetByUserAndLeaveTypeAsync(
        Guid userId,
        Guid leaveTypeId,
        int year,
        CancellationToken cancellationToken = default);
    
    Task<List<LeaveBalance>> GetListByUserIdAsync(
        Guid userId,
        int year,
        CancellationToken cancellationToken = default);
}
