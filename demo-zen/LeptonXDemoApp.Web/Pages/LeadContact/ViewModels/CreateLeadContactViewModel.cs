using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.LeadContact.ViewModels;

public class CreateLeadContactViewModel
{
    [Display(Name = "LeadContact:Name")]
    public string? Name { get; set; }
    [Display(Name = "LeadContact:Position")]
    public string? Position { get; set; }
    [Display(Name = "LeadContact:Email")]
    public string? Email { get; set; }
    [Display(Name = "LeadContact:Phone")]
    public string? Phone { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Display(Name = "LeadContact:LeadId")]
    [SelectItems(nameof(LeadList))]
    public Guid? LeadId { get; set; }

    public List<SelectListItem> LeadList { get; set; } = new();

    // ========== Child Collections (1:N Master-Detail) ==========
}
