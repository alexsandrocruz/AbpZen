using AutoMapper;
using Sapienza.Dominus.BlogCategory.Dtos;

namespace Sapienza.Dominus.BlogCategory;

public class BlogCategoryAutoMapperProfile : Profile
{
    public BlogCategoryAutoMapperProfile()
    {
        CreateMap<BlogCategory, BlogCategoryDto>();
        CreateMap<CreateUpdateBlogCategoryDto, BlogCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateBlogCategoryDto, BlogCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
