using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProVaras;
using Sapienza.Lexus.advProVaras.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProVaras;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProVarasFilterInput advProVarasFilter { get; set; }
    
    private readonly IadvProVarasAppService _advProVarasAppService;

    public IndexModel(IadvProVarasAppService advProVarasAppService)
    {
        _advProVarasAppService = advProVarasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProVarasGetListInput input)
    {
        var result = await _advProVarasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProVarasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProVarasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProVaras:idVara")]
    public int? idVara { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProVaras:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProVaras:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProVaras:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProVaras:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
