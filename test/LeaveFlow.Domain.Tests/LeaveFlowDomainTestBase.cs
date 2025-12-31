using Volo.Abp.Modularity;

namespace LeaveFlow;

/* Inherit from this class for your domain layer tests. */
public abstract class LeaveFlowDomainTestBase<TStartupModule> : LeaveFlowTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
