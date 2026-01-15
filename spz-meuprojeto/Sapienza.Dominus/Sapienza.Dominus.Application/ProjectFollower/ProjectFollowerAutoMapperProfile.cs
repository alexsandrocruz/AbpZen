using AutoMapper;
using Sapienza.Dominus.ProjectFollower.Dtos;

namespace Sapienza.Dominus.ProjectFollower;

public class ProjectFollowerAutoMapperProfile : Profile
{
    public ProjectFollowerAutoMapperProfile()
    {
        CreateMap<ProjectFollower, ProjectFollowerDto>()
            .ForMember(dest => dest.ProjectDisplayName, opt => opt.MapFrom(src => src.Project.Title));
        CreateMap<CreateUpdateProjectFollowerDto, ProjectFollower>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateProjectFollowerDto, ProjectFollower>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
