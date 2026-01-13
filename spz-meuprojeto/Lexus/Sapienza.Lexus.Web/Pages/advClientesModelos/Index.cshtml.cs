using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advClientesModelos;
using Sapienza.Lexus.advClientesModelos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advClientesModelos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advClientesModelosFilterInput advClientesModelosFilter { get; set; }
    
    private readonly IadvClientesModelosAppService _advClientesModelosAppService;

    public IndexModel(IadvClientesModelosAppService advClientesModelosAppService)
    {
        _advClientesModelosAppService = advClientesModelosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advClientesModelosGetListInput input)
    {
        var result = await _advClientesModelosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advClientesModelosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advClientesModelosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesModelos:idModelo")]
    public int? idModelo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesModelos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesModelos:conteudo")]
    public string? conteudo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesModelos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesModelos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesModelos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
