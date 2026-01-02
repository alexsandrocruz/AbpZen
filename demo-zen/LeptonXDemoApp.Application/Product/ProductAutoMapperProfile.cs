using AutoMapper;
using LeptonXDemoApp.Product.Dtos;

namespace LeptonXDemoApp.Product;

public class ProductAutoMapperProfile : Profile
{
    public ProductAutoMapperProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();
        CreateMap<CreateUpdateProductDto, Product>();
    }
}
