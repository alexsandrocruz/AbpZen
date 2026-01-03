using AutoMapper;
using LeptonXDemoApp.OrderItem.Dtos;
using LeptonXDemoApp.Web.Pages.OrderItem.ViewModels;

namespace LeptonXDemoApp.Web;

public class OrderItemWebAutoMapperProfile : Profile
{
    public OrderItemWebAutoMapperProfile()
    {
        CreateMap<OrderItemDto, EditOrderItemViewModel>();
        CreateMap<CreateOrderItemViewModel, CreateUpdateOrderItemDto>();
        CreateMap<EditOrderItemViewModel, CreateUpdateOrderItemDto>();
    }
}
