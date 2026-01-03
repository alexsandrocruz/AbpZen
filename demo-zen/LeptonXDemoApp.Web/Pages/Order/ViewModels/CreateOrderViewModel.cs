using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.OrderItem.Dtos;

namespace LeptonXDemoApp.Web.Pages.Order.ViewModels;

public class CreateOrderViewModel
{
    [Required]
    [Display(Name = "Order:Number")]
    public string Number { get; set; } = string.Empty;
    [Required]
    [Display(Name = "Order:Date")]
    public DateTime Date { get; set; }
    [Required]
    [Display(Name = "Order:Status")]
    public OrderStatus Status { get; set; }
    [Display(Name = "Order:Obs")]
    [TextArea(Rows = 3)]
    public string? Obs { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<CreateUpdateOrderItemDto> OrderItems { get; set; } = new();
}
