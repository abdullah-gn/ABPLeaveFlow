using Volo.Abp.Modularity;

namespace LeaveFlow;

[DependsOn(
    typeof(LeaveFlowDomainModule),
    typeof(LeaveFlowTestBaseModule)
)]
public class LeaveFlowDomainTestModule : AbpModule
{

}
