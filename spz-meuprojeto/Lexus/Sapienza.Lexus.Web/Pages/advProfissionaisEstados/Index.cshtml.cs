using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProfissionaisEstados;
using Sapienza.Lexus.advProfissionaisEstados.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProfissionaisEstados;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProfissionaisEstadosFilterInput advProfissionaisEstadosFilter { get; set; }
    
    private readonly IadvProfissionaisEstadosAppService _advProfissionaisEstadosAppService;

    public IndexModel(IadvProfissionaisEstadosAppService advProfissionaisEstadosAppService)
    {
        _advProfissionaisEstadosAppService = advProfissionaisEstadosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProfissionaisEstadosGetListInput input)
    {
        var result = await _advProfissionaisEstadosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProfissionaisEstadosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProfissionaisEstadosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionaisEstados:idProfissionalEstado")]
    public int? idProfissionalEstado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionaisEstados:idProfissional")]
    public int? idProfissional { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionaisEstados:estado")]
    public string? estado { get; set; }
}
