using AutoMapper;
using Sapienza.Dominus.TaskComment.Dtos;

namespace Sapienza.Dominus.TaskComment;

public class TaskCommentAutoMapperProfile : Profile
{
    public TaskCommentAutoMapperProfile()
    {
        CreateMap<TaskComment, TaskCommentDto>()
            .ForMember(dest => dest.TaskDisplayName, opt => opt.MapFrom(src => src.Task.Title));
        CreateMap<CreateUpdateTaskCommentDto, TaskComment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateTaskCommentDto, TaskComment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
