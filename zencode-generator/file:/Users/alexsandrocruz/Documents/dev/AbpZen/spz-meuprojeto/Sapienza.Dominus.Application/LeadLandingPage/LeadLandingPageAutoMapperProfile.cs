using AutoMapper;
using Sapienza.Dominus.LeadLandingPage.Dtos;

namespace Sapienza.Dominus.LeadLandingPage;

public class LeadLandingPageAutoMapperProfile : Profile
{
    public LeadLandingPageAutoMapperProfile()
    {
        CreateMap<LeadLandingPage, LeadLandingPageDto>()
            .ForMember(dest => dest.LeadWorkflowDisplayName, opt => opt.MapFrom(src => src.LeadWorkflow.Name));
        CreateMap<CreateUpdateLeadLandingPageDto, LeadLandingPage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadLandingPageDto, LeadLandingPage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
