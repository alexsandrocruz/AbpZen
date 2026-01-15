using AutoMapper;
using Sapienza.Dominus.SitePageVersion.Dtos;

namespace Sapienza.Dominus.SitePageVersion;

public class SitePageVersionAutoMapperProfile : Profile
{
    public SitePageVersionAutoMapperProfile()
    {
        CreateMap<SitePageVersion, SitePageVersionDto>()
            .ForMember(dest => dest.SitePageDisplayName, opt => opt.MapFrom(src => src.SitePage.Title));
        CreateMap<CreateUpdateSitePageVersionDto, SitePageVersion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateSitePageVersionDto, SitePageVersion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
