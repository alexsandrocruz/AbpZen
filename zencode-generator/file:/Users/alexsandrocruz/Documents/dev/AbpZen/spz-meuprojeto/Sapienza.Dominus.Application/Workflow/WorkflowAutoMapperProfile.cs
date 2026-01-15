using AutoMapper;
using Sapienza.Dominus.Workflow.Dtos;

namespace Sapienza.Dominus.Workflow;

public class WorkflowAutoMapperProfile : Profile
{
    public WorkflowAutoMapperProfile()
    {
        CreateMap<Workflow, WorkflowDto>();
        CreateMap<CreateUpdateWorkflowDto, Workflow>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateWorkflowDto, Workflow>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
