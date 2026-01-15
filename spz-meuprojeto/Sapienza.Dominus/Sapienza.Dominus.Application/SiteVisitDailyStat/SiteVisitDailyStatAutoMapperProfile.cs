using AutoMapper;
using Sapienza.Dominus.SiteVisitDailyStat.Dtos;

namespace Sapienza.Dominus.SiteVisitDailyStat;

public class SiteVisitDailyStatAutoMapperProfile : Profile
{
    public SiteVisitDailyStatAutoMapperProfile()
    {
        CreateMap<SiteVisitDailyStat, SiteVisitDailyStatDto>()
            .ForMember(dest => dest.SiteDisplayName, opt => opt.MapFrom(src => src.Site.Name));
        CreateMap<CreateUpdateSiteVisitDailyStatDto, SiteVisitDailyStat>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateSiteVisitDailyStatDto, SiteVisitDailyStat>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
