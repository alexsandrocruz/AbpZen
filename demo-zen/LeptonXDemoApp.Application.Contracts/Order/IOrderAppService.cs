using System;
using System.Threading.Tasks;
using LeptonXDemoApp.Order.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.Order;

public interface IOrderAppService :
    ICrudAppService<
        OrderDto,
        Guid,
        OrderGetListInput,
        CreateUpdateOrderDto,
        CreateUpdateOrderDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetOrderLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
