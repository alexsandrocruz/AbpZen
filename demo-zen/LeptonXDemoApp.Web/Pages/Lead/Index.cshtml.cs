using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.Lead;
using LeptonXDemoApp.Lead.Dtos;

namespace LeptonXDemoApp.Web.Pages.Lead;

public class IndexModel : LeptonXDemoAppPageModel
{
    public LeadFilterInput LeadFilter { get; set; }
    
    private readonly ILeadAppService _leadAppService;

    public IndexModel(ILeadAppService leadAppService)
    {
        _leadAppService = leadAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(LeadGetListInput input)
    {
        var result = await _leadAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _leadAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class LeadFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Lead:Name")]
    public string? Name { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Lead:Email")]
    public string? Email { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Lead:Phone")]
    public string? Phone { get; set; }
}
