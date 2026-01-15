using AutoMapper;
using Sapienza.Dominus.BlogPost.Dtos;

namespace Sapienza.Dominus.BlogPost;

public class BlogPostAutoMapperProfile : Profile
{
    public BlogPostAutoMapperProfile()
    {
        CreateMap<BlogPost, BlogPostDto>()
            .ForMember(dest => dest.SiteDisplayName, opt => opt.MapFrom(src => src.Site.Name));
        CreateMap<CreateUpdateBlogPostDto, BlogPost>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateBlogPostDto, BlogPost>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
