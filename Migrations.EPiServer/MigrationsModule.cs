using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Forte.Migrations.EPiServer
{
    [InitializableModule]
    [ModuleDependency(typeof(global::EPiServer.Web.InitializationModule))]
    public class MigrationsModule : IInitializableModule
    {
        private const string AppSettingsDisableKey = "fMigrationsDisableInit";
        
        public void Initialize(InitializationEngine context)
        {
            var disableInitFlag = System.Configuration.ConfigurationManager.AppSettings[AppSettingsDisableKey];
            
            if (disableInitFlag != null && bool.Parse(disableInitFlag)) return;
                
            var migrationRunner = new MigrationRunnerBuilder(context)
                .WithPrincipal("System", "Administrators")
                .Create();

            migrationRunner.RunMigrationsAsync().Wait();
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
