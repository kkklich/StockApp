using System.Collections.Generic;
using System.Threading.Tasks;
using Netsoftware.Xanthos.Common.Resources;

namespace Netsoftware.Xanthos.Common.Internationalities.Services.Interfaces;

public interface ICurrencyInfoService
{
    Task<int> CreateAsync(CurrencyInfoResource currencyInfoResource);
    Task<ICollection<CurrencyInfoResource>> ListAsync();
    Task<CurrencyInfoResource> GetAsyncByCode(string currencyCode);
}