using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LeptonXDemoApp.OrderItem.Dtos;

namespace LeptonXDemoApp.Order.Dtos;

[Serializable]
public class CreateUpdateOrderDto
{
    [Required]
    public string Number { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public OrderStatus Status { get; set; }
    public string Obs { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<CreateUpdateOrderItemDto> OrderItems { get; set; }
}
