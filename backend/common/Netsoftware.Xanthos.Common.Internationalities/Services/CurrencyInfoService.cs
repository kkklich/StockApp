using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Netsoftware.Xanthos.Common.Internationalities.Models;
using Netsoftware.Xanthos.Common.Internationalities.Repositories;
using Netsoftware.Xanthos.Common.Internationalities.Services.Interfaces;
using Netsoftware.Xanthos.Common.Resources;

namespace Netsoftware.Xanthos.Common.Internationalities.Services;

internal class CurrencyInfoService : ICurrencyInfoService
{
    private readonly IDocumentsGenericRepository<CurrencyInfo> _currencyInfoRepository;
    private readonly IMapper _mapper;

    public CurrencyInfoService(IDocumentsGenericRepository<CurrencyInfo> currencyInfoRepository, IMapper mapper)
    {
        _currencyInfoRepository = currencyInfoRepository;
        _mapper = mapper;
    }

    public async Task<int> CreateAsync(CurrencyInfoResource currencyInfoResource)
    {
        var currencyInfo = _mapper.Map<CurrencyInfo>(currencyInfoResource);
        await _currencyInfoRepository.CreateAsync(currencyInfo);
        return currencyInfo.Id;
    }

    public async Task<CurrencyInfoResource> GetAsyncByCode(string currencyCode)
    {
        var currencyInfo = (await _currencyInfoRepository.GetAsync(x => x.Code == currencyCode)).FirstOrDefault();
        if (currencyInfo == null) return null;
        var currencyInfoResource = _mapper.Map<CurrencyInfoResource>(currencyInfo);
        return currencyInfoResource;
    }

    public async Task<ICollection<CurrencyInfoResource>> ListAsync()
    {
        var currencyInfos = await _currencyInfoRepository.GetAsync();
        var currencyInfoResources = _mapper.Map<ICollection<CurrencyInfoResource>>(currencyInfos);
        return currencyInfoResources;
    }
}