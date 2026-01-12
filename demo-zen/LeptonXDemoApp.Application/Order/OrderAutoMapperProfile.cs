using AutoMapper;
using LeptonXDemoApp.Order.Dtos;

namespace LeptonXDemoApp.Order;

public class OrderAutoMapperProfile : Profile
{
    public OrderAutoMapperProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerDisplayName, opt => opt.MapFrom(src => src.Customer.Name));
        CreateMap<CreateUpdateOrderDto, Order>();
        CreateMap<CreateUpdateOrderDto, Order>();
    }
}
