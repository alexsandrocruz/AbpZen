using AutoMapper;
using LeptonXDemoApp.Customer.Dtos;
using LeptonXDemoApp.Web.Pages.Customer.ViewModels;

namespace LeptonXDemoApp.Web;

public class CustomerWebAutoMapperProfile : Profile
{
    public CustomerWebAutoMapperProfile()
    {
        CreateMap<CustomerDto, EditCustomerViewModel>();
        CreateMap<CreateCustomerViewModel, CreateUpdateCustomerDto>();
        CreateMap<EditCustomerViewModel, CreateUpdateCustomerDto>();
    }
}
