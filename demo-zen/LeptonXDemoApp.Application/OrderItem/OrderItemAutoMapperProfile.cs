using AutoMapper;
using LeptonXDemoApp.OrderItem.Dtos;

namespace LeptonXDemoApp.OrderItem;

public class OrderItemAutoMapperProfile : Profile
{
    public OrderItemAutoMapperProfile()
    {
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.OrderDisplayName, opt => opt.MapFrom(src => src.Order.Number));
        CreateMap<CreateUpdateOrderItemDto, OrderItem>();
        CreateMap<CreateUpdateOrderItemDto, OrderItem>();
    }
}
