using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LeaveFlow.Data;

/* This is used if database provider does't define
 * ILeaveFlowDbSchemaMigrator implementation.
 */
public class NullLeaveFlowDbSchemaMigrator : ILeaveFlowDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
