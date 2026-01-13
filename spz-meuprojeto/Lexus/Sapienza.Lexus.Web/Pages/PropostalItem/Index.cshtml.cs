using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.PropostalItem;
using Sapienza.Lexus.PropostalItem.Dtos;

namespace Sapienza.Lexus.Web.Pages.PropostalItem;

public class IndexModel : Sapienza.LexusPageModel
{
    public PropostalItemFilterInput PropostalItemFilter { get; set; }
    
    private readonly IPropostalItemAppService _propostalItemAppService;

    public IndexModel(IPropostalItemAppService propostalItemAppService)
    {
        _propostalItemAppService = propostalItemAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(PropostalItemGetListInput input)
    {
        var result = await _propostalItemAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _propostalItemAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class PropostalItemFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "PropostalItem:Desc")]
    public string? Desc { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "PropostalItem:Quant")]
    public decimal? Quant { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "PropostalItem:UnitPrice")]
    public decimal? UnitPrice { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "PropostalItem:Total")]
    public decimal? Total { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "PropostalItem:ProposalId")]
    public Guid? ProposalId { get; set; }
}
