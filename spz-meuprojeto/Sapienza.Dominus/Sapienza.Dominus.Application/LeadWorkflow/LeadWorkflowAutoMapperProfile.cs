using AutoMapper;
using Sapienza.Dominus.LeadWorkflow.Dtos;

namespace Sapienza.Dominus.LeadWorkflow;

public class LeadWorkflowAutoMapperProfile : Profile
{
    public LeadWorkflowAutoMapperProfile()
    {
        CreateMap<LeadWorkflow, LeadWorkflowDto>();
        CreateMap<CreateUpdateLeadWorkflowDto, LeadWorkflow>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadWorkflowDto, LeadWorkflow>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
