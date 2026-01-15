using AutoMapper;
using Sapienza.Dominus.Project.Dtos;

namespace Sapienza.Dominus.Project;

public class ProjectAutoMapperProfile : Profile
{
    public ProjectAutoMapperProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client.Name));
        CreateMap<CreateUpdateProjectDto, Project>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateProjectDto, Project>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
