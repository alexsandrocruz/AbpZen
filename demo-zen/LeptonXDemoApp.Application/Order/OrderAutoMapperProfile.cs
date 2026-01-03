using AutoMapper;
using LeptonXDemoApp.Order.Dtos;

namespace LeptonXDemoApp.Order;

public class OrderAutoMapperProfile : Profile
{
    public OrderAutoMapperProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<CreateUpdateOrderDto, Order>();
        CreateMap<CreateUpdateOrderDto, Order>();
    }
}
