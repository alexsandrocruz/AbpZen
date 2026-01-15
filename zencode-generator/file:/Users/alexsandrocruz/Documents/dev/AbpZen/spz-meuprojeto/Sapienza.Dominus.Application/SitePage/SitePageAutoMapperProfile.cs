using AutoMapper;
using Sapienza.Dominus.SitePage.Dtos;

namespace Sapienza.Dominus.SitePage;

public class SitePageAutoMapperProfile : Profile
{
    public SitePageAutoMapperProfile()
    {
        CreateMap<SitePage, SitePageDto>()
            .ForMember(dest => dest.SiteDisplayName, opt => opt.MapFrom(src => src.Site.Name));
        CreateMap<CreateUpdateSitePageDto, SitePage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateSitePageDto, SitePage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
