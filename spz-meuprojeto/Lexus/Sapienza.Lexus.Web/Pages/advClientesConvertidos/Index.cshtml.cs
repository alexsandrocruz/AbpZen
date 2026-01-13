using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advClientesConvertidos;
using Sapienza.Lexus.advClientesConvertidos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advClientesConvertidos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advClientesConvertidosFilterInput advClientesConvertidosFilter { get; set; }
    
    private readonly IadvClientesConvertidosAppService _advClientesConvertidosAppService;

    public IndexModel(IadvClientesConvertidosAppService advClientesConvertidosAppService)
    {
        _advClientesConvertidosAppService = advClientesConvertidosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advClientesConvertidosGetListInput input)
    {
        var result = await _advClientesConvertidosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advClientesConvertidosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advClientesConvertidosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesConvertidos:idRegistro")]
    public int? idRegistro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesConvertidos:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesConvertidos:data")]
    public string? data { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesConvertidos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesConvertidos:convertidoPor")]
    public string? convertidoPor { get; set; }
}
