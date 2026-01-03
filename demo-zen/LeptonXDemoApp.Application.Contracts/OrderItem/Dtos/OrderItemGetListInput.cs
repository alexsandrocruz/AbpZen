using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.OrderItem.Dtos;

[Serializable]
public class OrderItemGetListInput : PagedAndSortedResultRequestDto
{
    public Guid? ProductId { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? OrderId { get; set; }
}
