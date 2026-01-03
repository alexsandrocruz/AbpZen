using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.OrderItem.Dtos;

[Serializable]
public class CreateUpdateOrderItemDto
{
    [Required]
    public int Quant { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public decimal Total { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Required]
    public Guid OrderId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
