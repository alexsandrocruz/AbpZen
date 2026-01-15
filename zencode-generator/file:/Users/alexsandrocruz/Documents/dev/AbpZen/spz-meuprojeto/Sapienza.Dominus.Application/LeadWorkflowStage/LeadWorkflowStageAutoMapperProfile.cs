using AutoMapper;
using Sapienza.Dominus.LeadWorkflowStage.Dtos;

namespace Sapienza.Dominus.LeadWorkflowStage;

public class LeadWorkflowStageAutoMapperProfile : Profile
{
    public LeadWorkflowStageAutoMapperProfile()
    {
        CreateMap<LeadWorkflowStage, LeadWorkflowStageDto>()
            .ForMember(dest => dest.LeadWorkflowDisplayName, opt => opt.MapFrom(src => src.LeadWorkflow.Name));
        CreateMap<CreateUpdateLeadWorkflowStageDto, LeadWorkflowStage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadWorkflowStageDto, LeadWorkflowStage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
