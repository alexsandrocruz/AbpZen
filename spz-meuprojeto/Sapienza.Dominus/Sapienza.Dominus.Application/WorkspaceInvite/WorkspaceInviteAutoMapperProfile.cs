using AutoMapper;
using Sapienza.Dominus.WorkspaceInvite.Dtos;

namespace Sapienza.Dominus.WorkspaceInvite;

public class WorkspaceInviteAutoMapperProfile : Profile
{
    public WorkspaceInviteAutoMapperProfile()
    {
        CreateMap<WorkspaceInvite, WorkspaceInviteDto>();
        CreateMap<CreateUpdateWorkspaceInviteDto, WorkspaceInvite>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateWorkspaceInviteDto, WorkspaceInvite>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
