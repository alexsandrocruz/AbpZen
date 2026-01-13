using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPautaObs;
using Sapienza.Lexus.advPautaObs.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPautaObs;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPautaObsFilterInput advPautaObsFilter { get; set; }
    
    private readonly IadvPautaObsAppService _advPautaObsAppService;

    public IndexModel(IadvPautaObsAppService advPautaObsAppService)
    {
        _advPautaObsAppService = advPautaObsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPautaObsGetListInput input)
    {
        var result = await _advPautaObsAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPautaObsAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPautaObsFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPautaObs:idPautaObs")]
    public int? idPautaObs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPautaObs:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPautaObs:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPautaObs:id")]
    public int? id { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPautaObs:idTipo")]
    public string? idTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPautaObs:observacao")]
    public string? observacao { get; set; }
}
