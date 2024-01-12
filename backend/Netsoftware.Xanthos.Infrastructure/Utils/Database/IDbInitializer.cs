using System.Threading.Tasks;

namespace Netsoftware.Xanthos.Infrastructure.Utils.Database;

public interface IDbInitializer
{
    Task Initialize();
}