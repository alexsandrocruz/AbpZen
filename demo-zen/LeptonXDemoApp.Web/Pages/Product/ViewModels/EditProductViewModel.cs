using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.Product.ViewModels;

public class EditProductViewModel
{
    [Display(Name = "Product:Name")]
    public string? Name { get; set; }
    [Display(Name = "Product:Price")]
    public string? Price { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Display(Name = "Product:CategoryId")]
    [DynamicFormIgnore] // Hidden - rendered via abp-lookup-input
    public Guid? CategoryId { get; set; }

    [DynamicFormIgnore]
    public string? CategoryDisplayName { get; set; }
}
