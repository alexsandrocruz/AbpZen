using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using LeptonXDemoApp.OrderItem.Dtos;

namespace LeptonXDemoApp.Order.Dtos;

[Serializable]
public class OrderDto : FullAuditedEntityDto<Guid>
{
    public string Number { get; set; }
    public DateTime Date { get; set; }
    public OrderStatus Status { get; set; }
    public string Obs { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<OrderItemDto> OrderItems { get; set; }
}
