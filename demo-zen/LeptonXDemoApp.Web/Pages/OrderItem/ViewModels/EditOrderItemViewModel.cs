using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.OrderItem.ViewModels;

public class EditOrderItemViewModel
{
    [Required]
    [Display(Name = "OrderItem:Quant")]
    public int Quant { get; set; }
    [Required]
    [Display(Name = "OrderItem:Price")]
    public decimal Price { get; set; }
    [Required]
    [Display(Name = "OrderItem:Total")]
    public decimal Total { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Required]
    [Display(Name = "OrderItem:OrderId")]
    [SelectItems(nameof(OrderList))]
    public Guid OrderId { get; set; }

    public List<SelectListItem> OrderList { get; set; } = new();

    // ========== Child Collections (1:N Master-Detail) ==========
}
