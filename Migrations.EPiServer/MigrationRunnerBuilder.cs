using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using EPiServer.Security;
using EPiServer.ServiceLocation;

namespace Forte.Migrations.EPiServer
{
    public class MigrationRunnerBuilder
    {
        private IPrincipal principal;
        private readonly IServiceProvider serviceProvider;
        private IEnumerable<Assembly> assemblies;
        
        public MigrationRunnerBuilder(IServiceProvider serviceProvider, params Assembly[] assemblies)
        {
            this.serviceProvider = serviceProvider;
            this.assemblies = assemblies;
        }

        public MigrationRunnerBuilder WithAssemblies(params Assembly[] assemblies)
        {
            this.assemblies = this.assemblies != null ? this.assemblies.Concat(assemblies) : assemblies;
            return this;
        }

        public MigrationRunnerBuilder WithAssemblyOf<T>()
        {
            this.assemblies = this.assemblies != null
                ? this.assemblies.Concat(new[] {typeof(T).Assembly})
                : new[] {typeof(T).Assembly};
            
            return this;
        }
        
        public MigrationRunnerBuilder WithPrincipal(string userName, params string[] userRoles)
        {
            this.principal = CreatePrincipal(userName, userRoles);
            return this;
        }

        public MigrationRunner Create()
        {   
            var migrationsProvider = ReflectionMigrationProvider.FromAssemblies(this.assemblies);

            var activator = new DecoratingActivator<IMigration>(
                new ServiceProviderActivator(serviceProvider),
                new RunAsMigrationDecorator(this.principal ?? CreatePrincipal("System", new [] { "Administrators" }), 
                    serviceProvider.GetInstance<IPrincipalAccessor>()));

            return new MigrationRunner(
                migrationsProvider,
                new DynamicDataStoreMigrationLog(),
                serviceProvider.GetInstance<IMigrationSynchronizationContext>(),
                activator);
        }

        private static ClaimsPrincipal CreatePrincipal(string userName, IEnumerable<string> roles)
        {
            var userNameClaim = new Claim(ClaimTypes.Name, userName);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

            var systemIdentity = new ClaimsIdentity(roleClaims.Concat(new [] { userNameClaim }));
            
            return new ClaimsPrincipal(systemIdentity);
        }
    }
}
