using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;

namespace LeaveFlow.Data;

public class LeaveFlowPermissionDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IPermissionDataSeeder _permissionDataSeeder;

    public LeaveFlowPermissionDataSeederContributor(IPermissionDataSeeder permissionDataSeeder)
    {
        _permissionDataSeeder = permissionDataSeeder;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var permissions = new List<string>
        {
            // LeaveTypes
            "LeaveFlow.LeaveTypes",
            "LeaveFlow.LeaveTypes.Create",
            "LeaveFlow.LeaveTypes.Edit",
            "LeaveFlow.LeaveTypes.Delete",
            
            // LeaveRequests
            "LeaveFlow.LeaveRequests",
            "LeaveFlow.LeaveRequests.Create",
            "LeaveFlow.LeaveRequests.Edit",
            "LeaveFlow.LeaveRequests.Delete",
            "LeaveFlow.LeaveRequests.Approve",
            
            // LeaveBalances
            "LeaveFlow.LeaveBalances",
            "LeaveFlow.LeaveBalances.Create",
            "LeaveFlow.LeaveBalances.Edit"
        };

        // "R" = RolePermissionValueProvider, grants to role
        await _permissionDataSeeder.SeedAsync(
            "R",           // Provider name (Role)
            "admin",       // Provider key (role name)
            permissions,
            context.TenantId
        );
    }
}
