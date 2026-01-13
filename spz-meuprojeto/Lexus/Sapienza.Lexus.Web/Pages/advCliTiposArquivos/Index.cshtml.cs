using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCliTiposArquivos;
using Sapienza.Lexus.advCliTiposArquivos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCliTiposArquivos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCliTiposArquivosFilterInput advCliTiposArquivosFilter { get; set; }
    
    private readonly IadvCliTiposArquivosAppService _advCliTiposArquivosAppService;

    public IndexModel(IadvCliTiposArquivosAppService advCliTiposArquivosAppService)
    {
        _advCliTiposArquivosAppService = advCliTiposArquivosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCliTiposArquivosGetListInput input)
    {
        var result = await _advCliTiposArquivosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCliTiposArquivosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCliTiposArquivosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposArquivos:idTipoArquivo")]
    public int? idTipoArquivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposArquivos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposArquivos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposArquivos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposArquivos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliTiposArquivos:pasta")]
    public string? pasta { get; set; }
}
