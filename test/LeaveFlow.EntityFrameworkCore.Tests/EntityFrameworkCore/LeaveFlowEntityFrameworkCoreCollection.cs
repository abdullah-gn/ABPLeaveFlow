using Xunit;

namespace LeaveFlow.EntityFrameworkCore;

[CollectionDefinition(LeaveFlowTestConsts.CollectionDefinitionName)]
public class LeaveFlowEntityFrameworkCoreCollection : ICollectionFixture<LeaveFlowEntityFrameworkCoreFixture>
{

}
