using AutoMapper;
using Sapienza.Dominus.ProjectCommunication.Dtos;

namespace Sapienza.Dominus.ProjectCommunication;

public class ProjectCommunicationAutoMapperProfile : Profile
{
    public ProjectCommunicationAutoMapperProfile()
    {
        CreateMap<ProjectCommunication, ProjectCommunicationDto>()
            .ForMember(dest => dest.ProjectDisplayName, opt => opt.MapFrom(src => src.Project.Title));
        CreateMap<CreateUpdateProjectCommunicationDto, ProjectCommunication>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateProjectCommunicationDto, ProjectCommunication>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
