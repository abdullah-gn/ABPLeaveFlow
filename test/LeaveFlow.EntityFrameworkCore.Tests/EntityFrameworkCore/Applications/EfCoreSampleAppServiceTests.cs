using LeaveFlow.Samples;
using Xunit;

namespace LeaveFlow.EntityFrameworkCore.Applications;

[Collection(LeaveFlowTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<LeaveFlowEntityFrameworkCoreTestModule>
{

}
