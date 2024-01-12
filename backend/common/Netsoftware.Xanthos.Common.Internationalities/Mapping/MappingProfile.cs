using AutoMapper;
using Netsoftware.Xanthos.Common.Internationalities.Models;
using Netsoftware.Xanthos.Common.Resources;

namespace Netsoftware.Xanthos.Common.Internationalities.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CultureInfo, CultureInfoResource>();
        CreateMap<CultureInfoResource, CultureInfo>();
        CreateMap<CurrencyInfo, CurrencyInfoResource>();
        CreateMap<CurrencyInfoResource, CurrencyInfo>();
    }
}