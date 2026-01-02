using AutoMapper;
using LeptonXDemoApp.Product.Dtos;
using LeptonXDemoApp.Web.Pages.Product.ViewModels;

namespace LeptonXDemoApp.Web;

public class ProductWebAutoMapperProfile : Profile
{
    public ProductWebAutoMapperProfile()
    {
        CreateMap<ProductDto, EditProductViewModel>();
        CreateMap<CreateProductViewModel, CreateUpdateProductDto>();
        CreateMap<EditProductViewModel, CreateUpdateProductDto>();
    }
}
