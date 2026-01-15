using AutoMapper;
using Sapienza.Dominus.LeadAutomation.Dtos;

namespace Sapienza.Dominus.LeadAutomation;

public class LeadAutomationAutoMapperProfile : Profile
{
    public LeadAutomationAutoMapperProfile()
    {
        CreateMap<LeadAutomation, LeadAutomationDto>()
            .ForMember(dest => dest.LeadWorkflowDisplayName, opt => opt.MapFrom(src => src.LeadWorkflow.Name));
        CreateMap<CreateUpdateLeadAutomationDto, LeadAutomation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadAutomationDto, LeadAutomation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
