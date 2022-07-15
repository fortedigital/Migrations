using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Forte.Migrations.EPiServer
{
    public static class MigrationsModuleExtensions
    {
        public static IServiceCollection AddMigrations(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.TryAddTransient<IMigrationSynchronizationContext, DatabaseLockedSynchronizationContext>();
            
            return services;
        }
        
        public static IApplicationBuilder RunMigrations(this IApplicationBuilder builder, params Assembly[] assemblies)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var serviceProvider = builder.ApplicationServices.GetRequiredService<IServiceProvider>();
                
            var migrationRunner = new MigrationRunnerBuilder(serviceProvider, assemblies)
                .WithPrincipal("System", "Administrators")
                .Create();

            migrationRunner.RunMigrationsAsync().Wait();
            return builder;
        }
    }
}