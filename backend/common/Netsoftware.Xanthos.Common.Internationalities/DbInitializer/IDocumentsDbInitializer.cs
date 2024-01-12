using System.Threading.Tasks;

namespace Netsoftware.Xanthos.Common.Internationalities.DbInitializer;

internal interface IDocumentsDbInitializer
{
    Task Initialize();
}