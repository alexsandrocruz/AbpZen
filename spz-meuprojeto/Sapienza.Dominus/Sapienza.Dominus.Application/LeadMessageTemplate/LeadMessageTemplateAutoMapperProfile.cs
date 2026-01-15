using AutoMapper;
using Sapienza.Dominus.LeadMessageTemplate.Dtos;

namespace Sapienza.Dominus.LeadMessageTemplate;

public class LeadMessageTemplateAutoMapperProfile : Profile
{
    public LeadMessageTemplateAutoMapperProfile()
    {
        CreateMap<LeadMessageTemplate, LeadMessageTemplateDto>()
            .ForMember(dest => dest.LeadWorkflowDisplayName, opt => opt.MapFrom(src => src.LeadWorkflow.Name));
        CreateMap<CreateUpdateLeadMessageTemplateDto, LeadMessageTemplate>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadMessageTemplateDto, LeadMessageTemplate>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
