using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.OrderItem.Dtos;

[Serializable]
public class OrderItemDto : FullAuditedEntityDto<Guid>
{
    public int Quant { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid OrderId { get; set; }
    public string? OrderDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
