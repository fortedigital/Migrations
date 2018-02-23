using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forte.Migrations
{
    public interface IMigrationProvider
    {
        IEnumerable<MigrationDescriptor> LoadMigrations();
    }
}