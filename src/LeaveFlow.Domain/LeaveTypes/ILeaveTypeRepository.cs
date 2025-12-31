using System;
using Volo.Abp.Domain.Repositories;

namespace LeaveFlow.LeaveTypes;

public interface ILeaveTypeRepository : IRepository<LeaveType, Guid>
{
    // Add custom repository methods here if needed
}
