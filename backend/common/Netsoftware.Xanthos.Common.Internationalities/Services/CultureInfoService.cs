using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Netsoftware.Xanthos.Common.Internationalities.Models;
using Netsoftware.Xanthos.Common.Internationalities.Repositories;
using Netsoftware.Xanthos.Common.Internationalities.Services.Interfaces;
using Netsoftware.Xanthos.Common.Resources;

namespace Netsoftware.Xanthos.Common.Internationalities.Services;

internal class CultureInfoService : ICultureInfoService
{
    private readonly IDocumentsGenericRepository<CultureInfo> _cultureInfoRepository;
    private readonly IMapper _mapper;

    public CultureInfoService(IDocumentsGenericRepository<CultureInfo> cultureInfoRepository, IMapper mapper)
    {
        _cultureInfoRepository = cultureInfoRepository;
        _mapper = mapper;
    }

    public async Task<int> CreateAsync(CultureInfoResource cultureInfoResource)
    {
        var cultureInfo = _mapper.Map<CultureInfo>(cultureInfoResource);
        await _cultureInfoRepository.CreateAsync(cultureInfo);
        return cultureInfo.Id;
    }

    public async Task<CultureInfoResource> GetAsyncBySpecCulture(string SpecCulture)
    {
        var cultureInfo = (await _cultureInfoRepository.GetAsync(x => x.SpecCulture == SpecCulture)).FirstOrDefault();
        if (cultureInfo == null) return null;
        var cultureInfoResource = _mapper.Map<CultureInfoResource>(cultureInfo);
        return cultureInfoResource;
    }

    public async Task<ICollection<CultureInfoResource>> ListAsync()
    {
        var cultureInfos = await _cultureInfoRepository.GetAsync();
        var cultureInfoResources = _mapper.Map<ICollection<CultureInfoResource>>(cultureInfos);
        return cultureInfoResources;
    }
}