using AutoMapper;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Resources;

namespace Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Database.Models.ApplicationInstance, ApplicationInstanceResource>();
    }
}