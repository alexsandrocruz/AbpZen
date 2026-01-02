using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.Order.ViewModels;

public class EditOrderViewModel
{
    [Display(Name = "Order:Number")]
    public string? Number { get; set; }
    [Display(Name = "Order:Date")]
    public DateTime? Date { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Display(Name = "Order:CustomerId")]
    [DynamicFormIgnore] // Hidden - rendered via abp-lookup-input
    public Guid? CustomerId { get; set; }

    [DynamicFormIgnore]
    public string? CustomerDisplayName { get; set; }
}
