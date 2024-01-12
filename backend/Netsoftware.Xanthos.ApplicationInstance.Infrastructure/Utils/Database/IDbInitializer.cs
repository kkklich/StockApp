using System.Threading.Tasks;

namespace Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Utils.Database;

public interface IDbInitializer
{
    Task Initialize();
}