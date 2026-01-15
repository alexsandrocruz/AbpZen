using AutoMapper;
using Sapienza.Dominus.Product.Dtos;

namespace Sapienza.Dominus.Product;

public class ProductAutoMapperProfile : Profile
{
    public ProductAutoMapperProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
