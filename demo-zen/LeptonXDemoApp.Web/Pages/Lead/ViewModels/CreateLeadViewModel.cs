using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.Lead.ViewModels;

public class CreateLeadViewModel
{
    [Required]
    [Display(Name = "Lead:Name")]
    public string Name { get; set; } = string.Empty;
    [Display(Name = "Lead:Email")]
    public string? Email { get; set; }
    [Display(Name = "Lead:Phone")]
    public string? Phone { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
