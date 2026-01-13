using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProEscritorios;
using Sapienza.Lexus.advProEscritorios.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProEscritorios;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProEscritoriosFilterInput advProEscritoriosFilter { get; set; }
    
    private readonly IadvProEscritoriosAppService _advProEscritoriosAppService;

    public IndexModel(IadvProEscritoriosAppService advProEscritoriosAppService)
    {
        _advProEscritoriosAppService = advProEscritoriosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProEscritoriosGetListInput input)
    {
        var result = await _advProEscritoriosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProEscritoriosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProEscritoriosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProEscritorios:idEscritorio")]
    public int? idEscritorio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProEscritorios:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProEscritorios:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProEscritorios:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProEscritorios:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProEscritorios:idCentroCusto")]
    public int? idCentroCusto { get; set; }
}
