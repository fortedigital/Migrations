using System;
using System.Threading.Tasks;
using EPiServer.Data;
using EPiServer.ServiceLocation;

namespace Forte.Migrations.EPiServer
{
    [ServiceConfiguration(ServiceType = typeof(IMigrationSynchronizationContext))]
    public class DatabaseLockedSynchronizationContext : IMigrationSynchronizationContext
    {
        private readonly IDatabaseExecutor executor;

        public DatabaseLockedSynchronizationContext(IDatabaseExecutor executor)
        {
            this.executor = executor;
        }

        public Task<TResult> RunSynchronizedAsync<TResult>(Func<Task<TResult>> action)
        {
            return Task.FromResult(this.executor.ExecuteLocked("Migrations", () => action().Result));
        }
    }
}