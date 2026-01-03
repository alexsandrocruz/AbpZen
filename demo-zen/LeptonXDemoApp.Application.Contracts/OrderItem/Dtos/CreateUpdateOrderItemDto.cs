using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.OrderItem.Dtos;

[Serializable]
public class CreateUpdateOrderItemDto
{
    public decimal? Quant { get; set; }
    public decimal? Price { get; set; }
    public decimal? Total { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProductId { get; set; }
    public Guid? OrderId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
