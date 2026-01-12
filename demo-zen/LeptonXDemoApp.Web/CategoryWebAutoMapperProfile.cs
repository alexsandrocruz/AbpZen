using AutoMapper;
using LeptonXDemoApp.Category.Dtos;
using LeptonXDemoApp.Web.Pages.Category.ViewModels;

namespace LeptonXDemoApp.Web;

public class CategoryWebAutoMapperProfile : Profile
{
    public CategoryWebAutoMapperProfile()
    {
        CreateMap<CategoryDto, EditCategoryViewModel>();
        CreateMap<CreateCategoryViewModel, CreateUpdateCategoryDto>();
        CreateMap<EditCategoryViewModel, CreateUpdateCategoryDto>();
    }
}
