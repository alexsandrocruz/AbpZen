using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.LeadContact;
using LeptonXDemoApp.LeadContact.Dtos;

namespace LeptonXDemoApp.Web.Pages.LeadContact;

public class IndexModel : LeptonXDemoAppPageModel
{
    public LeadContactFilterInput LeadContactFilter { get; set; }
    
    private readonly ILeadContactAppService _leadContactAppService;

    public IndexModel(ILeadContactAppService leadContactAppService)
    {
        _leadContactAppService = leadContactAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(LeadContactGetListInput input)
    {
        var result = await _leadContactAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _leadContactAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class LeadContactFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadContact:Name")]
    public string? Name { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadContact:Position")]
    public string? Position { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadContact:Email")]
    public string? Email { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadContact:Phone")]
    public string? Phone { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadContact:LeadId")]
    public Guid? LeadId { get; set; }
}
