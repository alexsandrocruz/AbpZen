using AutoMapper;
using Sapienza.Dominus.Task.Dtos;

namespace Sapienza.Dominus.Task;

public class TaskAutoMapperProfile : Profile
{
    public TaskAutoMapperProfile()
    {
        CreateMap<Task, TaskDto>()
            .ForMember(dest => dest.ProjectDisplayName, opt => opt.MapFrom(src => src.Project.Title));
        CreateMap<CreateUpdateTaskDto, Task>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateTaskDto, Task>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
