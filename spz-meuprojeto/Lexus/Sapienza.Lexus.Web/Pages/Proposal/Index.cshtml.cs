using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.Proposal;
using Sapienza.Lexus.Proposal.Dtos;

namespace Sapienza.Lexus.Web.Pages.Proposal;

public class IndexModel : Sapienza.LexusPageModel
{
    public ProposalFilterInput ProposalFilter { get; set; }
    
    private readonly IProposalAppService _proposalAppService;

    public IndexModel(IProposalAppService proposalAppService)
    {
        _proposalAppService = proposalAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(ProposalGetListInput input)
    {
        var result = await _proposalAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _proposalAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class ProposalFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Proposal:Number")]
    public string? Number { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Proposal:Date")]
    public DateTime? Date { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Proposal:Validate")]
    public DateTime? Validate { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Proposal:Obs")]
    public string? Obs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Proposal:ClientId")]
    public Guid? ClientId { get; set; }
}
