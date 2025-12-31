using Volo.Abp.Modularity;

namespace LeaveFlow;

[DependsOn(
    typeof(LeaveFlowApplicationModule),
    typeof(LeaveFlowDomainTestModule)
)]
public class LeaveFlowApplicationTestModule : AbpModule
{

}
