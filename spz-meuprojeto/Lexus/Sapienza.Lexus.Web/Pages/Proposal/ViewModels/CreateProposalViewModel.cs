using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.Proposal.ViewModels;

public class CreateProposalViewModel
{
    [Display(Name = "Proposal:Number")]
    public string? Number { get; set; }
    [Display(Name = "Proposal:Date")]
    public DateTime? Date { get; set; }
    [Display(Name = "Proposal:Validate")]
    public DateTime? Validate { get; set; }
    [Display(Name = "Proposal:Obs")]
    [TextArea(Rows = 3)]
    public string? Obs { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Display(Name = "Proposal:ClientId")]
    [DynamicFormIgnore] // Hidden - rendered via abp-lookup-input
    public Guid? ClientId { get; set; }

    [DynamicFormIgnore]
    public string? ClientDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
