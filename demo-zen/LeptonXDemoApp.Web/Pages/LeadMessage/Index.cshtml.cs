using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.LeadMessage;
using LeptonXDemoApp.LeadMessage.Dtos;

namespace LeptonXDemoApp.Web.Pages.LeadMessage;

public class IndexModel : LeptonXDemoAppPageModel
{
    public LeadMessageFilterInput LeadMessageFilter { get; set; }
    
    private readonly ILeadMessageAppService _leadMessageAppService;

    public IndexModel(ILeadMessageAppService leadMessageAppService)
    {
        _leadMessageAppService = leadMessageAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(LeadMessageGetListInput input)
    {
        var result = await _leadMessageAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _leadMessageAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class LeadMessageFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadMessage:Title")]
    public string? Title { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadMessage:MessageTemplateId")]
    public Guid? MessageTemplateId { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadMessage:Date")]
    public DateTime? Date { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadMessage:LeadId")]
    public Guid? LeadId { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadMessage:Body")]
    public string? Body { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LeadMessage:Success")]
    public bool? Success { get; set; }
}
