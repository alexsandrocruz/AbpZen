using AutoMapper;
using Sapienza.Dominus.Lead.Dtos;

namespace Sapienza.Dominus.Lead;

public class LeadAutoMapperProfile : Profile
{
    public LeadAutoMapperProfile()
    {
        CreateMap<Lead, LeadDto>()
            .ForMember(dest => dest.LeadWorkflowDisplayName, opt => opt.MapFrom(src => src.LeadWorkflow.Name))
            .ForMember(dest => dest.LeadWorkflowStageDisplayName, opt => opt.MapFrom(src => src.LeadWorkflowStage.Name));
        CreateMap<CreateUpdateLeadDto, Lead>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadDto, Lead>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
