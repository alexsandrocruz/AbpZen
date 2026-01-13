using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabFormasRecebimento;
using Sapienza.Lexus.fabFormasRecebimento.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabFormasRecebimento;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabFormasRecebimentoFilterInput fabFormasRecebimentoFilter { get; set; }
    
    private readonly IfabFormasRecebimentoAppService _fabFormasRecebimentoAppService;

    public IndexModel(IfabFormasRecebimentoAppService fabFormasRecebimentoAppService)
    {
        _fabFormasRecebimentoAppService = fabFormasRecebimentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabFormasRecebimentoGetListInput input)
    {
        var result = await _fabFormasRecebimentoAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabFormasRecebimentoAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabFormasRecebimentoFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:idFormaRecebimento")]
    public int? idFormaRecebimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:ordem")]
    public int? ordem { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:padrao")]
    public bool? padrao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:idCondicaoPagamento")]
    public int? idCondicaoPagamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:online")]
    public bool? online { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:tipo")]
    public string? tipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:emailPagSeguro")]
    public string? emailPagSeguro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:texto")]
    public string? texto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:contasReceber")]
    public bool? contasReceber { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:vendas")]
    public bool? vendas { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:diasParaPrevisao")]
    public int? diasParaPrevisao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:valorDesconto")]
    public double? valorDesconto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:descontoTipo")]
    public string? descontoTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:recebimentoFuturo")]
    public bool? recebimentoFuturo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:recebimentoFuturoDias")]
    public int? recebimentoFuturoDias { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:recebimentoFuturoTaxa")]
    public double? recebimentoFuturoTaxa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:idConta")]
    public int? idConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:idPlanoConta")]
    public int? idPlanoConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:idCentroCusto")]
    public int? idCentroCusto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:idContaPagar")]
    public int? idContaPagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:idPlanoContaPagar")]
    public int? idPlanoContaPagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:idCentroCustoPagar")]
    public int? idCentroCustoPagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasRecebimento:idFormaPagar")]
    public int? idFormaPagar { get; set; }
}
