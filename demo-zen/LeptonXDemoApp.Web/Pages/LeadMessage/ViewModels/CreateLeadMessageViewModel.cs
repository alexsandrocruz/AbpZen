using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.LeadMessage.ViewModels;

public class CreateLeadMessageViewModel
{
    [Display(Name = "LeadMessage:Title")]
    public string? Title { get; set; }
    [Display(Name = "LeadMessage:Date")]
    public DateTime? Date { get; set; }
    [Display(Name = "LeadMessage:Body")]
    [TextArea(Rows = 3)]
    public string? Body { get; set; }
    [Display(Name = "LeadMessage:Success")]
    public bool? Success { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Display(Name = "LeadMessage:MessageTemplateId")]
    [SelectItems(nameof(MessageTemplateList))]
    public Guid? MessageTemplateId { get; set; }

    public List<SelectListItem> MessageTemplateList { get; set; } = new();
    [Display(Name = "LeadMessage:LeadId")]
    [SelectItems(nameof(LeadList))]
    public Guid? LeadId { get; set; }

    public List<SelectListItem> LeadList { get; set; } = new();

    // ========== Child Collections (1:N Master-Detail) ==========
}
