using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advAgeTiposTarefas;
using Sapienza.Lexus.advAgeTiposTarefas.Dtos;

namespace Sapienza.Lexus.Web.Pages.advAgeTiposTarefas;

public class IndexModel : Sapienza.LexusPageModel
{
    public advAgeTiposTarefasFilterInput advAgeTiposTarefasFilter { get; set; }
    
    private readonly IadvAgeTiposTarefasAppService _advAgeTiposTarefasAppService;

    public IndexModel(IadvAgeTiposTarefasAppService advAgeTiposTarefasAppService)
    {
        _advAgeTiposTarefasAppService = advAgeTiposTarefasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advAgeTiposTarefasGetListInput input)
    {
        var result = await _advAgeTiposTarefasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advAgeTiposTarefasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advAgeTiposTarefasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposTarefas:idTipoTarefa")]
    public int? idTipoTarefa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposTarefas:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposTarefas:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposTarefas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposTarefas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposTarefas:agendada")]
    public bool? agendada { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advAgeTiposTarefas:pauta")]
    public bool? pauta { get; set; }
}
