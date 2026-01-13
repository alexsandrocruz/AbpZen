using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advClientesHistoricos;
using Sapienza.Lexus.advClientesHistoricos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advClientesHistoricos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advClientesHistoricosFilterInput advClientesHistoricosFilter { get; set; }
    
    private readonly IadvClientesHistoricosAppService _advClientesHistoricosAppService;

    public IndexModel(IadvClientesHistoricosAppService advClientesHistoricosAppService)
    {
        _advClientesHistoricosAppService = advClientesHistoricosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advClientesHistoricosGetListInput input)
    {
        var result = await _advClientesHistoricosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advClientesHistoricosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advClientesHistoricosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:idHistorico")]
    public int? idHistorico { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:idTipoHistorico")]
    public int? idTipoHistorico { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:data")]
    public string? data { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:hora")]
    public string? hora { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:ocorrencia")]
    public string? ocorrencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:idOportunidade")]
    public int? idOportunidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:depto")]
    public string? depto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesHistoricos:prioritario")]
    public bool? prioritario { get; set; }
}
