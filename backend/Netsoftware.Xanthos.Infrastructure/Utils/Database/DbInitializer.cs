using System.Threading.Tasks;

namespace Netsoftware.Xanthos.Infrastructure.Utils.Database;

public class DbInitializer : IDbInitializer
{
    public async Task Initialize()
    {
        await SeedSomeData();
    }

    private async Task SeedSomeData()
    {
        await Task.CompletedTask;
    }
}