using AutoMapper;
using LeptonXDemoApp.Order.Dtos;
using LeptonXDemoApp.Web.Pages.Order.ViewModels;

namespace LeptonXDemoApp.Web;

public class OrderWebAutoMapperProfile : Profile
{
    public OrderWebAutoMapperProfile()
    {
        CreateMap<OrderDto, EditOrderViewModel>();
        CreateMap<CreateOrderViewModel, CreateUpdateOrderDto>();
        CreateMap<EditOrderViewModel, CreateUpdateOrderDto>();
    }
}
