using System.Security.Principal;
using System.Threading.Tasks;
using EPiServer.Security;

namespace Forte.Migrations.EPiServer
{
    public class RunAsMigrationDecorator : IDecoratorProvider<IMigration>
    {
        private readonly IPrincipal principal;
        private readonly IPrincipalAccessor principalAccessor;

        public RunAsMigrationDecorator(IPrincipal principal, IPrincipalAccessor principalAccessor)
        {
            this.principal = principal;
            this.principalAccessor = principalAccessor;
        }

        public IMigration Decorate(IMigration instance)
        {
            return new Decorator(instance, principal, principalAccessor);
        }

        private class Decorator : IMigration
        {
            private readonly IPrincipal principal;
            private readonly IPrincipalAccessor principalAccessor;
            private readonly IMigration innerMigration;

            public Decorator(IMigration innerMigration, IPrincipal principal, IPrincipalAccessor principalAccessor)
            {
                this.innerMigration = innerMigration;
                this.principal = principal;
                this.principalAccessor = principalAccessor;
            }

            public async Task ExecuteAsync()
            {
                var oldPrincipal = PrincipalInfo.CurrentPrincipal;
                
                principalAccessor.Principal = this.principal;
                
                try
                {
                    await this.innerMigration.ExecuteAsync();
                }
                finally
                {
                    principalAccessor.Principal = oldPrincipal;
                }
            }
        }
    }
}