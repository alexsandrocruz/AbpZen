using AutoMapper;
using Sapienza.Dominus.WorkspaceAccessEvent.Dtos;

namespace Sapienza.Dominus.WorkspaceAccessEvent;

public class WorkspaceAccessEventAutoMapperProfile : Profile
{
    public WorkspaceAccessEventAutoMapperProfile()
    {
        CreateMap<WorkspaceAccessEvent, WorkspaceAccessEventDto>();
        CreateMap<CreateUpdateWorkspaceAccessEventDto, WorkspaceAccessEvent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateWorkspaceAccessEventDto, WorkspaceAccessEvent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
