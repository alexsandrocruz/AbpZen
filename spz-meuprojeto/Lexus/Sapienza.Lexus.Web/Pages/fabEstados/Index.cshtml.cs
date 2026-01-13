using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabEstados;
using Sapienza.Lexus.fabEstados.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabEstados;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabEstadosFilterInput fabEstadosFilter { get; set; }
    
    private readonly IfabEstadosAppService _fabEstadosAppService;

    public IndexModel(IfabEstadosAppService fabEstadosAppService)
    {
        _fabEstadosAppService = fabEstadosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabEstadosGetListInput input)
    {
        var result = await _fabEstadosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabEstadosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabEstadosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabEstados:idEstado")]
    public int? idEstado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabEstados:sigla")]
    public string? sigla { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabEstados:descricao")]
    public string? descricao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabEstados:idPais")]
    public int? idPais { get; set; }
}
