using AutoMapper;
using LeptonXDemoApp.Product.Dtos;
using LeptonXDemoApp.Web.Pages.Product.ViewModels;

namespace LeptonXDemoApp.Web;

public class ProductWebAutoMapperProfile : Profile
{
    public ProductWebAutoMapperProfile()
    {
        CreateMap<ProductDto, EditProductViewModel>()
            .ForMember(dest => dest.CategoryDisplayName, opt => opt.MapFrom(src => src.CategoryDisplayName));
        CreateMap<CreateProductViewModel, CreateUpdateProductDto>();
        CreateMap<EditProductViewModel, CreateUpdateProductDto>();
    }
}
