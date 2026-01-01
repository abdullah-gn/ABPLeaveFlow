using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LeaveFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeaveFlow.LeaveTypes;

[Authorize(LeaveFlowPermissions.LeaveTypes.Default)]
public class LeaveTypeAppService :
    CrudAppService<
        LeaveType,
        LeaveTypeDto,
        Guid,
        GetLeaveTypeListDto,
        CreateUpdateLeaveTypeDto>,
    ILeaveTypeAppService
{
    public LeaveTypeAppService(IRepository<LeaveType, Guid> repository)
        : base(repository)
    {
        GetPolicyName = LeaveFlowPermissions.LeaveTypes.Default;
        GetListPolicyName = LeaveFlowPermissions.LeaveTypes.Default;
        CreatePolicyName = LeaveFlowPermissions.LeaveTypes.Create;
        UpdatePolicyName = LeaveFlowPermissions.LeaveTypes.Edit;
        DeletePolicyName = LeaveFlowPermissions.LeaveTypes.Delete;
    }

    public async Task<List<LeaveTypeDto>> GetActiveListAsync()
    {
        var queryable = await Repository.GetQueryableAsync();
        
        var leaveTypes = await AsyncExecuter.ToListAsync(
            queryable.Where(x => x.IsActive)
        );

        return ObjectMapper.Map<List<LeaveType>, List<LeaveTypeDto>>(leaveTypes);
    }

    protected override async Task<IQueryable<LeaveType>> CreateFilteredQueryAsync(GetLeaveTypeListDto input)
    {
        var queryable = await Repository.GetQueryableAsync();

        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), 
                x => x.Name.Contains(input.Filter!) || 
                     (x.Description != null && x.Description.Contains(input.Filter!)))
            .WhereIf(input.IsActive.HasValue, 
                x => x.IsActive == input.IsActive!.Value);
    }

    protected override async Task<LeaveType> MapToEntityAsync(CreateUpdateLeaveTypeDto createInput)
    {
        return new LeaveType(
            GuidGenerator.Create(),
            createInput.Name,
            createInput.DefaultDays,
            createInput.IsPaid
        )
        {
            Description = createInput.Description,
            IsActive = createInput.IsActive
        };
    }

    protected override async Task MapToEntityAsync(CreateUpdateLeaveTypeDto updateInput, LeaveType entity)
    {
        entity.SetName(updateInput.Name);
        entity.Description = updateInput.Description;
        entity.DefaultDays = updateInput.DefaultDays;
        entity.IsPaid = updateInput.IsPaid;
        entity.IsActive = updateInput.IsActive;
    }
}
