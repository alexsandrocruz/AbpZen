using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.PropostalItem.ViewModels;

public class EditPropostalItemViewModel
{
    [Display(Name = "PropostalItem:Desc")]
    public string? Desc { get; set; }
    [Display(Name = "PropostalItem:Quant")]
    public decimal? Quant { get; set; }
    [Display(Name = "PropostalItem:UnitPrice")]
    public decimal? UnitPrice { get; set; }
    [Display(Name = "PropostalItem:Total")]
    public decimal? Total { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Display(Name = "PropostalItem:ProposalId")]
    [SelectItems(nameof(ProposalList))]
    public Guid? ProposalId { get; set; }

    public List<SelectListItem> ProposalList { get; set; } = new();

    // ========== Child Collections (1:N Master-Detail) ==========
}
