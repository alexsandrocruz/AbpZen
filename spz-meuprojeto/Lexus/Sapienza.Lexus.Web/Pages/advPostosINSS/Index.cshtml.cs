using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPostosINSS;
using Sapienza.Lexus.advPostosINSS.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPostosINSS;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPostosINSSFilterInput advPostosINSSFilter { get; set; }
    
    private readonly IadvPostosINSSAppService _advPostosINSSAppService;

    public IndexModel(IadvPostosINSSAppService advPostosINSSAppService)
    {
        _advPostosINSSAppService = advPostosINSSAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPostosINSSGetListInput input)
    {
        var result = await _advPostosINSSAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPostosINSSAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPostosINSSFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPostosINSS:idPosto")]
    public int? idPosto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPostosINSS:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPostosINSS:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPostosINSS:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPostosINSS:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
