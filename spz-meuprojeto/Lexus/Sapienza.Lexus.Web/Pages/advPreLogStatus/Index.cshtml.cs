using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPreLogStatus;
using Sapienza.Lexus.advPreLogStatus.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPreLogStatus;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPreLogStatusFilterInput advPreLogStatusFilter { get; set; }
    
    private readonly IadvPreLogStatusAppService _advPreLogStatusAppService;

    public IndexModel(IadvPreLogStatusAppService advPreLogStatusAppService)
    {
        _advPreLogStatusAppService = advPreLogStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPreLogStatusGetListInput input)
    {
        var result = await _advPreLogStatusAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPreLogStatusAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPreLogStatusFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreLogStatus:idLog")]
    public int? idLog { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreLogStatus:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreLogStatus:idStatus")]
    public int? idStatus { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreLogStatus:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreLogStatus:conversao")]
    public bool? conversao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreLogStatus:tsConversao")]
    public DateTime? tsConversao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreLogStatus:perdido")]
    public bool? perdido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreLogStatus:tsPerdido")]
    public DateTime? tsPerdido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreLogStatus:diasCorridosDoAnterior")]
    public int? diasCorridosDoAnterior { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreLogStatus:usuario")]
    public string? usuario { get; set; }
}
