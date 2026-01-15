using AutoMapper;
using Sapienza.Dominus.BlogPostVersion.Dtos;

namespace Sapienza.Dominus.BlogPostVersion;

public class BlogPostVersionAutoMapperProfile : Profile
{
    public BlogPostVersionAutoMapperProfile()
    {
        CreateMap<BlogPostVersion, BlogPostVersionDto>()
            .ForMember(dest => dest.BlogPostDisplayName, opt => opt.MapFrom(src => src.BlogPost.Title));
        CreateMap<CreateUpdateBlogPostVersionDto, BlogPostVersion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateBlogPostVersionDto, BlogPostVersion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
