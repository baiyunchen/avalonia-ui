using System.Threading.Tasks;

namespace Sysware.Data;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}


