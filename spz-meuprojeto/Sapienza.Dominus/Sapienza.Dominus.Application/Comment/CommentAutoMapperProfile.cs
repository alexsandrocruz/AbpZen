using AutoMapper;
using Sapienza.Dominus.Comment.Dtos;

namespace Sapienza.Dominus.Comment;

public class CommentAutoMapperProfile : Profile
{
    public CommentAutoMapperProfile()
    {
        CreateMap<Comment, CommentDto>();
        CreateMap<CreateUpdateCommentDto, Comment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateCommentDto, Comment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
