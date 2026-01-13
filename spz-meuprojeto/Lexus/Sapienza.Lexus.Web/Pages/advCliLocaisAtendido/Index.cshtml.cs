using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCliLocaisAtendido;
using Sapienza.Lexus.advCliLocaisAtendido.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCliLocaisAtendido;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCliLocaisAtendidoFilterInput advCliLocaisAtendidoFilter { get; set; }
    
    private readonly IadvCliLocaisAtendidoAppService _advCliLocaisAtendidoAppService;

    public IndexModel(IadvCliLocaisAtendidoAppService advCliLocaisAtendidoAppService)
    {
        _advCliLocaisAtendidoAppService = advCliLocaisAtendidoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCliLocaisAtendidoGetListInput input)
    {
        var result = await _advCliLocaisAtendidoAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCliLocaisAtendidoAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCliLocaisAtendidoFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLocaisAtendido:idLocalAtendido")]
    public int? idLocalAtendido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLocaisAtendido:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLocaisAtendido:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLocaisAtendido:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLocaisAtendido:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
