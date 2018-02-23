using System.Threading.Tasks;

namespace Forte.Migrations
{
    public interface IMigration
    {
        Task ExecuteAsync();
    }
}