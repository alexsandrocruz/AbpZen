using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabDatasEFeriados;
using Sapienza.Lexus.fabDatasEFeriados.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabDatasEFeriados;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabDatasEFeriadosFilterInput fabDatasEFeriadosFilter { get; set; }
    
    private readonly IfabDatasEFeriadosAppService _fabDatasEFeriadosAppService;

    public IndexModel(IfabDatasEFeriadosAppService fabDatasEFeriadosAppService)
    {
        _fabDatasEFeriadosAppService = fabDatasEFeriadosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabDatasEFeriadosGetListInput input)
    {
        var result = await _fabDatasEFeriadosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabDatasEFeriadosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabDatasEFeriadosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabDatasEFeriados:idData")]
    public int? idData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabDatasEFeriados:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabDatasEFeriados:data")]
    public string? data { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabDatasEFeriados:feriado")]
    public bool? feriado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabDatasEFeriados:fixo")]
    public bool? fixo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabDatasEFeriados:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabDatasEFeriados:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabDatasEFeriados:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
