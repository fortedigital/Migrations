using System.Linq;
using System.Threading.Tasks;

namespace Forte.Migrations
{
    public class MigrationRunner
    {
        private readonly IMigrationProvider migrationProvider;
        private readonly IMigrationLog migrationLog;
        private readonly IMigrationSynchronizationContext synchronizationContext;
        private readonly IActivator<IMigration> activator;

        public MigrationRunner(IMigrationProvider migrationProvider, IMigrationLog migrationLog, IMigrationSynchronizationContext synchronizationContext)
            :this(migrationProvider, migrationLog, synchronizationContext, new ReflectionActivator<IMigration>())
        {
        }

        public MigrationRunner(IMigrationProvider migrationProvider, IMigrationLog migrationLog, IMigrationSynchronizationContext synchronizationContext, IActivator<IMigration> activator)
        {
            this.migrationProvider = migrationProvider;
            this.migrationLog = migrationLog;
            this.activator = activator;
            this.synchronizationContext = synchronizationContext;
        }

        public async Task RunMigrationsAsync()
        {
            var availableMigrations = this.migrationProvider.LoadMigrations();
            var orderedMigrations = new MigrationSorter(availableMigrations).Sort();

            await this.synchronizationContext.RunSynchronizedAsync(async () =>
            {
                var pendingMigrations = orderedMigrations.Where(m => this.migrationLog.Contains(m.Id) == false);
                foreach (var migrationDescriptor in pendingMigrations)
                {
                    var migration = migrationDescriptor.CreateMigration(this.activator);
                    await migration.ExecuteAsync();
                    this.migrationLog.Add(migrationDescriptor);
                }
            });
        }
    }
}