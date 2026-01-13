using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabCidades;
using Sapienza.Lexus.fabCidades.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabCidades;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabCidadesFilterInput fabCidadesFilter { get; set; }
    
    private readonly IfabCidadesAppService _fabCidadesAppService;

    public IndexModel(IfabCidadesAppService fabCidadesAppService)
    {
        _fabCidadesAppService = fabCidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabCidadesGetListInput input)
    {
        var result = await _fabCidadesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabCidadesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabCidadesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCidades:idCidade")]
    public int? idCidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCidades:descricao")]
    public string? descricao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCidades:codigoIBGE")]
    public string? codigoIBGE { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCidades:idEstado")]
    public int? idEstado { get; set; }
}
