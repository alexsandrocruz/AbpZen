using AutoMapper;
using Sapienza.Dominus.Site.Dtos;

namespace Sapienza.Dominus.Site;

public class SiteAutoMapperProfile : Profile
{
    public SiteAutoMapperProfile()
    {
        CreateMap<Site, SiteDto>();
        CreateMap<CreateUpdateSiteDto, Site>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateSiteDto, Site>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
