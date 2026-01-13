using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProcessosDadosHerdeiros;
using Sapienza.Lexus.advProcessosDadosHerdeiros.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProcessosDadosHerdeiros;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProcessosDadosHerdeirosFilterInput advProcessosDadosHerdeirosFilter { get; set; }
    
    private readonly IadvProcessosDadosHerdeirosAppService _advProcessosDadosHerdeirosAppService;

    public IndexModel(IadvProcessosDadosHerdeirosAppService advProcessosDadosHerdeirosAppService)
    {
        _advProcessosDadosHerdeirosAppService = advProcessosDadosHerdeirosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProcessosDadosHerdeirosGetListInput input)
    {
        var result = await _advProcessosDadosHerdeirosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProcessosDadosHerdeirosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProcessosDadosHerdeirosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:idHerdeiro")]
    public int? idHerdeiro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:sequencia")]
    public int? sequencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:bancarioBancoId")]
    public int? bancarioBancoId { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:bancarioTipoConta")]
    public string? bancarioTipoConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:bancarioAgencia")]
    public string? bancarioAgencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:bancarioConta")]
    public string? bancarioConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:bancarioFavorecido")]
    public string? bancarioFavorecido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:bancarioCpf")]
    public string? bancarioCpf { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:bancarioPerc")]
    public double? bancarioPerc { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:bancarioTarifa")]
    public double? bancarioTarifa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:bancarioTarifaParcelas")]
    public string? bancarioTarifaParcelas { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosDadosHerdeiros:idHonorario")]
    public int? idHonorario { get; set; }
}
