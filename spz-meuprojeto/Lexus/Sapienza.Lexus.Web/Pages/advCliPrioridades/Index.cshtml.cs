using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCliPrioridades;
using Sapienza.Lexus.advCliPrioridades.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCliPrioridades;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCliPrioridadesFilterInput advCliPrioridadesFilter { get; set; }
    
    private readonly IadvCliPrioridadesAppService _advCliPrioridadesAppService;

    public IndexModel(IadvCliPrioridadesAppService advCliPrioridadesAppService)
    {
        _advCliPrioridadesAppService = advCliPrioridadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCliPrioridadesGetListInput input)
    {
        var result = await _advCliPrioridadesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCliPrioridadesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCliPrioridadesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliPrioridades:idPrioridade")]
    public int? idPrioridade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliPrioridades:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliPrioridades:cor")]
    public string? cor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliPrioridades:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliPrioridades:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliPrioridades:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
