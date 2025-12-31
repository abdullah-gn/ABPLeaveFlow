using LeaveFlow.Samples;
using Xunit;

namespace LeaveFlow.EntityFrameworkCore.Domains;

[Collection(LeaveFlowTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<LeaveFlowEntityFrameworkCoreTestModule>
{

}
