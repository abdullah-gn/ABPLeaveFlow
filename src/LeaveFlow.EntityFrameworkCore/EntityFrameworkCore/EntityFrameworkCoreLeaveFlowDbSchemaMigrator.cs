using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LeaveFlow.Data;
using Volo.Abp.DependencyInjection;

namespace LeaveFlow.EntityFrameworkCore;

public class EntityFrameworkCoreLeaveFlowDbSchemaMigrator
    : ILeaveFlowDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreLeaveFlowDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the LeaveFlowDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<LeaveFlowDbContext>()
            .Database
            .MigrateAsync();
    }
}
