using System.Collections.Generic;
using System.Threading.Tasks;
using Netsoftware.Xanthos.Common.Resources;

namespace Netsoftware.Xanthos.Common.Internationalities.Services.Interfaces;

public interface ICultureInfoService
{
    Task<int> CreateAsync(CultureInfoResource cultureInfoResource);
    Task<ICollection<CultureInfoResource>> ListAsync();
    Task<CultureInfoResource> GetAsyncBySpecCulture(string SpecCulture);
}