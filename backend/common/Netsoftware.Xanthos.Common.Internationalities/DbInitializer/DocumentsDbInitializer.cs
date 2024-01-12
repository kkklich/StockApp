using System.Threading.Tasks;
using Netsoftware.Xanthos.Common.Internationalities.Services.Interfaces;

namespace Netsoftware.Xanthos.Common.Internationalities.DbInitializer;

internal class DocumentsDbInitializer : IDocumentsDbInitializer
{
    private readonly ICultureInfoService _cultureInfo;
    private readonly ICurrencyInfoService _currencyInfo;

    public DocumentsDbInitializer(ICultureInfoService cultureInfo, ICurrencyInfoService currencyInfo)
    {
        _cultureInfo = cultureInfo;
        _currencyInfo = currencyInfo;
    }

    public async Task Initialize()
    {        
    }   
}