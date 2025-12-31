using LeaveFlow.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LeaveFlow.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(LeaveFlowEntityFrameworkCoreModule),
    typeof(LeaveFlowApplicationContractsModule)
    )]
public class LeaveFlowDbMigratorModule : AbpModule
{
}
