using System;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;

namespace Forte.Migrations.EPiServer
{
    [InitializableModule]
    [ModuleDependency(typeof(global::EPiServer.Web.InitializationModule))]
    public class MigrationsModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {

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