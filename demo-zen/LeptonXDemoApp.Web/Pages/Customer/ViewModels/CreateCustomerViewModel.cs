using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.Customer.ViewModels;

public class CreateCustomerViewModel
{
    [Display(Name = "Customer:Name")]
    public string? Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
}
