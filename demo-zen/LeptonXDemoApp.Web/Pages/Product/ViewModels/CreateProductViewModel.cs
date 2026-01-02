using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.Product.ViewModels;

public class CreateProductViewModel
{
    [Display(Name = "Product:Name")]
    public string? Name { get; set; }
    [Display(Name = "Product:Price")]
    public string? Price { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Display(Name = "Product:CategoryId")]
    // Lookup mode: Modal - Rendering manual lookup input recommended
    public Guid? CategoryId { get; set; }

    public string? CategoryDisplayName { get; set; }
}
