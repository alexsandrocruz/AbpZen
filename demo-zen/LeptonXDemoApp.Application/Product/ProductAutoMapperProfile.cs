using AutoMapper;
using LeptonXDemoApp.Product.Dtos;

namespace LeptonXDemoApp.Product;

public class ProductAutoMapperProfile : Profile
{
    public ProductAutoMapperProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryDisplayName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<CreateUpdateProductDto, Product>();
        CreateMap<CreateUpdateProductDto, Product>();
    }
}
