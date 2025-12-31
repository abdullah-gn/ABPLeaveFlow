using System.Threading.Tasks;

namespace LeaveFlow.Data;

public interface ILeaveFlowDbSchemaMigrator
{
    Task MigrateAsync();
}
