using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProProbabilidades;
using Sapienza.Lexus.advProProbabilidades.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProProbabilidades;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProProbabilidadesFilterInput advProProbabilidadesFilter { get; set; }
    
    private readonly IadvProProbabilidadesAppService _advProProbabilidadesAppService;

    public IndexModel(IadvProProbabilidadesAppService advProProbabilidadesAppService)
    {
        _advProProbabilidadesAppService = advProProbabilidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProProbabilidadesGetListInput input)
    {
        var result = await _advProProbabilidadesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProProbabilidadesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProProbabilidadesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProProbabilidades:idProbabilidade")]
    public int? idProbabilidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProProbabilidades:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProProbabilidades:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProProbabilidades:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProProbabilidades:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
