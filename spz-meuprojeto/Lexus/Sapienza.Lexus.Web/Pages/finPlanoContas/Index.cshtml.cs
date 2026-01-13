using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finPlanoContas;
using Sapienza.Lexus.finPlanoContas.Dtos;

namespace Sapienza.Lexus.Web.Pages.finPlanoContas;

public class IndexModel : Sapienza.LexusPageModel
{
    public finPlanoContasFilterInput finPlanoContasFilter { get; set; }
    
    private readonly IfinPlanoContasAppService _finPlanoContasAppService;

    public IndexModel(IfinPlanoContasAppService finPlanoContasAppService)
    {
        _finPlanoContasAppService = finPlanoContasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finPlanoContasGetListInput input)
    {
        var result = await _finPlanoContasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finPlanoContasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finPlanoContasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:idPlanoConta")]
    public int? idPlanoConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:idGrupo")]
    public int? idGrupo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:codigo")]
    public string? codigo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:pagamentoSempreLiberado")]
    public bool? pagamentoSempreLiberado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:permiteLancamentoQuitado")]
    public bool? permiteLancamentoQuitado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:padraoVendas")]
    public bool? padraoVendas { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:antecipaVencimento")]
    public bool? antecipaVencimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:terceiroNivel")]
    public bool? terceiroNivel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:criarPeloFinanceiro")]
    public bool? criarPeloFinanceiro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:valoresRestritos")]
    public bool? valoresRestritos { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContas:naoAbatePagtoDoSaldoDoCliente")]
    public bool? naoAbatePagtoDoSaldoDoCliente { get; set; }
}
