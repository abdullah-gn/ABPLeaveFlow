using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeaveFlow.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeaveFlow.LeaveBalances;

public class EfCoreLeaveBalanceRepository 
    : EfCoreRepository<LeaveFlowDbContext, LeaveBalance, Guid>, ILeaveBalanceRepository
{
    public EfCoreLeaveBalanceRepository(IDbContextProvider<LeaveFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<LeaveBalance?> GetByUserAndLeaveTypeAsync(
        Guid userId,
        Guid leaveTypeId,
        int year,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        
        return await dbSet
            .Include(x => x.LeaveType)
            .FirstOrDefaultAsync(
                x => x.UserId == userId && x.LeaveTypeId == leaveTypeId && x.Year == year,
                cancellationToken);
    }

    public async Task<List<LeaveBalance>> GetListByUserIdAsync(
        Guid userId,
        int year,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        
        return await dbSet
            .Where(x => x.UserId == userId && x.Year == year)
            .Include(x => x.LeaveType)
            .ToListAsync(cancellationToken);
    }
}
