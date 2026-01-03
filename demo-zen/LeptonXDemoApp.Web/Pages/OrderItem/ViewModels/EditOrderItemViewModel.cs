using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.OrderItem.ViewModels;

public class EditOrderItemViewModel
{
    [Display(Name = "OrderItem:Quant")]
    public decimal? Quant { get; set; }
    [Display(Name = "OrderItem:Price")]
    public decimal? Price { get; set; }
    [Display(Name = "OrderItem:Total")]
    public decimal? Total { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Display(Name = "OrderItem:ProductId")]
    [SelectItems(nameof(ProductList))]
    public Guid? ProductId { get; set; }

    public List<SelectListItem> ProductList { get; set; } = new();
    [Display(Name = "OrderItem:OrderId")]
    [SelectItems(nameof(OrderList))]
    public Guid? OrderId { get; set; }

    public List<SelectListItem> OrderList { get; set; } = new();

    // ========== Child Collections (1:N Master-Detail) ==========
}
