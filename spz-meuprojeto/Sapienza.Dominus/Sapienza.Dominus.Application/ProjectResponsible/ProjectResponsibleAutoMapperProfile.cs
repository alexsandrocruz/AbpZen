using AutoMapper;
using Sapienza.Dominus.ProjectResponsible.Dtos;

namespace Sapienza.Dominus.ProjectResponsible;

public class ProjectResponsibleAutoMapperProfile : Profile
{
    public ProjectResponsibleAutoMapperProfile()
    {
        CreateMap<ProjectResponsible, ProjectResponsibleDto>()
            .ForMember(dest => dest.ProjectDisplayName, opt => opt.MapFrom(src => src.Project.Title));
        CreateMap<CreateUpdateProjectResponsibleDto, ProjectResponsible>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateProjectResponsibleDto, ProjectResponsible>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
