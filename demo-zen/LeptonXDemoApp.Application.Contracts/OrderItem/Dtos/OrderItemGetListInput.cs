using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.OrderItem.Dtos;

[Serializable]
public class OrderItemGetListInput : PagedAndSortedResultRequestDto
{
    public decimal? Quant { get; set; }
    public decimal? Price { get; set; }
    public decimal? Total { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ProductId { get; set; }
    public Guid? OrderId { get; set; }
}
