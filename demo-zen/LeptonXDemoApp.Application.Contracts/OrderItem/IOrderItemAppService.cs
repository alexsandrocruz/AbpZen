using System;
using System.Threading.Tasks;
using LeptonXDemoApp.OrderItem.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.OrderItem;

public interface IOrderItemAppService :
    ICrudAppService<
        OrderItemDto,
        Guid,
        OrderItemGetListInput,
        CreateUpdateOrderItemDto,
        CreateUpdateOrderItemDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetOrderItemLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
