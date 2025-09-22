using System.Threading.Tasks;

namespace Sysware.Data.Migrations;

public interface IMigration
{
    string Id { get; }
    string Name { get; }
    Task UpAsync();
}


