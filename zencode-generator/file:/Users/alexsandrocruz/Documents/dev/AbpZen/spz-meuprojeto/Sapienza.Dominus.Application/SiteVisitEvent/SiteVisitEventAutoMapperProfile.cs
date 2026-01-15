using AutoMapper;
using Sapienza.Dominus.SiteVisitEvent.Dtos;

namespace Sapienza.Dominus.SiteVisitEvent;

public class SiteVisitEventAutoMapperProfile : Profile
{
    public SiteVisitEventAutoMapperProfile()
    {
        CreateMap<SiteVisitEvent, SiteVisitEventDto>()
            .ForMember(dest => dest.SiteDisplayName, opt => opt.MapFrom(src => src.Site.Name));
        CreateMap<CreateUpdateSiteVisitEventDto, SiteVisitEvent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateSiteVisitEventDto, SiteVisitEvent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
