using System.Collections.Generic;
using AutoMapper;
using Netsoftware.Xanthos.Common.EmailSender;
using Netsoftware.Xanthos.EmailQueue.Database.Models;
using Newtonsoft.Json;

namespace Netsoftware.Xanthos.EmailQueue.Infrastructure.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EmailQueueResource, EmailQueueModel>()
            .ForMember(x => x.Receivers, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Receivers)))
            .ForMember(x => x.FileDownloadUrls,
                opt => opt.MapFrom(src =>
                    src.FileDownloadUrls != null ? JsonConvert.SerializeObject(src.FileDownloadUrls) : null));
        CreateMap<EmailQueueModel, EmailQueueResource>()
            .ForMember(x => x.Receivers,
                opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<EmailAddress>>(src.Receivers)))
            .ForMember(x => x.FileDownloadUrls,
                opt => opt.MapFrom(src =>
                    src.FileDownloadUrls != null
                        ? JsonConvert.DeserializeObject<List<string>>(src.FileDownloadUrls)
                        : null));
    }
}