using AutoMapper;
using Sapienza.Dominus.WorkflowExecution.Dtos;

namespace Sapienza.Dominus.WorkflowExecution;

public class WorkflowExecutionAutoMapperProfile : Profile
{
    public WorkflowExecutionAutoMapperProfile()
    {
        CreateMap<WorkflowExecution, WorkflowExecutionDto>()
            .ForMember(dest => dest.WorkflowDisplayName, opt => opt.MapFrom(src => src.Workflow.Name));
        CreateMap<CreateUpdateWorkflowExecutionDto, WorkflowExecution>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateWorkflowExecutionDto, WorkflowExecution>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
