using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeaveFlow.LeaveTypes;

public interface ILeaveTypeAppService : ICrudAppService<
    LeaveTypeDto,
    Guid,
    GetLeaveTypeListDto,
    CreateUpdateLeaveTypeDto>
{
    Task<List<LeaveTypeDto>> GetActiveListAsync();
}
