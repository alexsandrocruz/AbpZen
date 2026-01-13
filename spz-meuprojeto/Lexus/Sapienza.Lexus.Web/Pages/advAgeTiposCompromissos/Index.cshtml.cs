using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advAgeTiposCompromissos;
using Sapienza.Lexus.advAgeTiposCompromissos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advAgeTiposCompromissos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advAgeTiposCompromissosFilterInput advAgeTiposCompromissosFilter { get; set; }
    
    private readonly IadvAgeTiposCompromissosAppService _advAgeTiposCompromissosAppService;

    public IndexModel(IadvAgeTiposCompromissosAppService advAgeTiposCompromissosAppService)
    {
        _advAgeTiposCompromissosAppService = advAgeTiposCompromissosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advAgeTiposCompromissosGetListInput input)
    {
        var result = await _advAgeTiposCompromissosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advAgeTiposCompromissosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advAgeTiposCompromissosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposCompromissos:idTipoCompromisso")]
    public int? idTipoCompromisso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposCompromissos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposCompromissos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposCompromissos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposCompromissos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposCompromissos:recebimentoProcesso")]
    public bool? recebimentoProcesso { get; set; }
}
