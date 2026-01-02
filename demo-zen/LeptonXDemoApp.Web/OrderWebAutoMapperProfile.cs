using AutoMapper;
using LeptonXDemoApp.Order.Dtos;
using LeptonXDemoApp.Web.Pages.Order.ViewModels;

namespace LeptonXDemoApp.Web;

public class OrderWebAutoMapperProfile : Profile
{
    public OrderWebAutoMapperProfile()
    {
        CreateMap<OrderDto, EditOrderViewModel>()
            .ForMember(dest => dest.CustomerDisplayName, opt => opt.MapFrom(src => src.CustomerDisplayName));
        CreateMap<CreateOrderViewModel, CreateUpdateOrderDto>();
        CreateMap<EditOrderViewModel, CreateUpdateOrderDto>();
    }
}
