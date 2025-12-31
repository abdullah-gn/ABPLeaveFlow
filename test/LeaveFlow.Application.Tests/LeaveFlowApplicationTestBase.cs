using Volo.Abp.Modularity;

namespace LeaveFlow;

public abstract class LeaveFlowApplicationTestBase<TStartupModule> : LeaveFlowTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
