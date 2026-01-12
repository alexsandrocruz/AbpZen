using AutoMapper;
using LeptonXDemoApp.Customer.Dtos;

namespace LeptonXDemoApp.Customer;

public class CustomerAutoMapperProfile : Profile
{
    public CustomerAutoMapperProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>();
        CreateMap<CreateUpdateCustomerDto, Customer>();
    }
}
