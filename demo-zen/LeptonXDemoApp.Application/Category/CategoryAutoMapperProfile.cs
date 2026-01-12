using AutoMapper;
using LeptonXDemoApp.Category.Dtos;

namespace LeptonXDemoApp.Category;

public class CategoryAutoMapperProfile : Profile
{
    public CategoryAutoMapperProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateUpdateCategoryDto, Category>();
        CreateMap<CreateUpdateCategoryDto, Category>();
    }
}
