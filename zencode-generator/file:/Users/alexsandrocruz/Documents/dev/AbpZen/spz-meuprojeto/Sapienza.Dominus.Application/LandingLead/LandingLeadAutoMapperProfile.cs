using AutoMapper;
using Sapienza.Dominus.LandingLead.Dtos;

namespace Sapienza.Dominus.LandingLead;

public class LandingLeadAutoMapperProfile : Profile
{
    public LandingLeadAutoMapperProfile()
    {
        CreateMap<LandingLead, LandingLeadDto>();
        CreateMap<CreateUpdateLandingLeadDto, LandingLead>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLandingLeadDto, LandingLead>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
