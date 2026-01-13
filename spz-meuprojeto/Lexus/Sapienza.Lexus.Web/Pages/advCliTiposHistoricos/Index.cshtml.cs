using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCliTiposHistoricos;
using Sapienza.Lexus.advCliTiposHistoricos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCliTiposHistoricos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCliTiposHistoricosFilterInput advCliTiposHistoricosFilter { get; set; }
    
    private readonly IadvCliTiposHistoricosAppService _advCliTiposHistoricosAppService;

    public IndexModel(IadvCliTiposHistoricosAppService advCliTiposHistoricosAppService)
    {
        _advCliTiposHistoricosAppService = advCliTiposHistoricosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCliTiposHistoricosGetListInput input)
    {
        var result = await _advCliTiposHistoricosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCliTiposHistoricosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCliTiposHistoricosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposHistoricos:idTipoHistorico")]
    public int? idTipoHistorico { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposHistoricos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposHistoricos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposHistoricos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposHistoricos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
