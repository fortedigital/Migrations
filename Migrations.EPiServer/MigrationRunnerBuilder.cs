using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using EPiServer.Framework.Initialization;

namespace Forte.Migrations.EPiServer
{
    public class MigrationRunnerBuilder
    {
        private IPrincipal principal;
        private IEnumerable<Assembly> assemblies;

        private readonly InitializationEngine context;

        public MigrationRunnerBuilder(InitializationEngine context)
        {
            this.context = context;
        }

        public MigrationRunnerBuilder WithAssemblies(params Assembly[] assemblies)
        {
            this.assemblies = assemblies;
            return this;
        }

        public MigrationRunnerBuilder WithAssemblyOf<T>()
        {
            this.assemblies = new [] { typeof(T).Assembly };
            return this;
        }
        
        public MigrationRunnerBuilder WithPrincipal(string userName, params string[] userRoles)
        {
            this.principal = CreatePrincipal(userName, userRoles);
            return this;
        }

        public MigrationRunner Create()
        {
            return new MigrationRunner(
                ReflectionMigrationProvider.FromAssemblies(this.assemblies ?? context.Assemblies),
                new DynamicDataStoreMigrationLog(),
                context.Locate.Advanced.GetInstance<DatabaseLockedSynchronizationContext>(),
                new DecoratingActivator<IMigration>(new RunAsMigrationDecorator(this.principal ?? CreatePrincipal("System", new [] { "Administrators" }))));
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