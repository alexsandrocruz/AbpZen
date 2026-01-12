using System;
using System.Threading.Tasks;
using LeptonXDemoApp.Customer.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.Customer;

public interface ICustomerAppService :
    ICrudAppService<
        CustomerDto,
        Guid,
        CustomerGetListInput,
        CreateUpdateCustomerDto,
        CreateUpdateCustomerDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetCustomerLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
