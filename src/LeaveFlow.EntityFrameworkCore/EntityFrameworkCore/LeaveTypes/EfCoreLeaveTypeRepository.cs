using System;
using LeaveFlow.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeaveFlow.LeaveTypes;

public class EfCoreLeaveTypeRepository 
    : EfCoreRepository<LeaveFlowDbContext, LeaveType, Guid>, ILeaveTypeRepository
{
    public EfCoreLeaveTypeRepository(IDbContextProvider<LeaveFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
