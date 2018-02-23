using System.Security.Principal;
using System.Threading.Tasks;
using EPiServer.Security;

namespace Forte.Migrations.EPiServer
{
    public class RunAsMigrationDecorator : IDecoratorProvider<IMigration>
    {
        private readonly IPrincipal principal;

        public RunAsMigrationDecorator(IPrincipal principal)
        {
            this.principal = principal;
        }

        public IMigration Decorate(IMigration instance)
        {
            return new Decorator(instance, principal);
        }

        private class Decorator : IMigration
        {
            private readonly IPrincipal principal;
            private readonly IMigration innerMigration;

            public Decorator(IMigration innerMigration, IPrincipal principal)
            {
                this.innerMigration = innerMigration;
                this.principal = principal;
            }

            public async Task ExecuteAsync()
            {
                var oldPrincipal = PrincipalInfo.CurrentPrincipal;
                PrincipalInfo.CurrentPrincipal = this.principal;
                try
                {
                    await this.innerMigration.ExecuteAsync();
                }
                finally
                {
                    PrincipalInfo.CurrentPrincipal = oldPrincipal;
                }
            }
        }
    }
}