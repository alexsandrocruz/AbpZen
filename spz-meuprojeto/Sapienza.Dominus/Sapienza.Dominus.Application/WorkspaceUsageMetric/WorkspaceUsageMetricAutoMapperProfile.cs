using AutoMapper;
using Sapienza.Dominus.WorkspaceUsageMetric.Dtos;

namespace Sapienza.Dominus.WorkspaceUsageMetric;

public class WorkspaceUsageMetricAutoMapperProfile : Profile
{
    public WorkspaceUsageMetricAutoMapperProfile()
    {
        CreateMap<WorkspaceUsageMetric, WorkspaceUsageMetricDto>();
        CreateMap<CreateUpdateWorkspaceUsageMetricDto, WorkspaceUsageMetric>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateWorkspaceUsageMetricDto, WorkspaceUsageMetric>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
