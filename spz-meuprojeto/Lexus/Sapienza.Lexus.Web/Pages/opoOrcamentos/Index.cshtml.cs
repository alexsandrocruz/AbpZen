using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.opoOrcamentos;
using Sapienza.Lexus.opoOrcamentos.Dtos;

namespace Sapienza.Lexus.Web.Pages.opoOrcamentos;

public class IndexModel : Sapienza.LexusPageModel
{
    public opoOrcamentosFilterInput opoOrcamentosFilter { get; set; }
    
    private readonly IopoOrcamentosAppService _opoOrcamentosAppService;

    public IndexModel(IopoOrcamentosAppService opoOrcamentosAppService)
    {
        _opoOrcamentosAppService = opoOrcamentosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(opoOrcamentosGetListInput input)
    {
        var result = await _opoOrcamentosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _opoOrcamentosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class opoOrcamentosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:idOrcamento")]
    public int? idOrcamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:idOportunidade")]
    public int? idOportunidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:dataCriacao")]
    public string? dataCriacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valor")]
    public double? valor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:arquivo")]
    public string? arquivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:aceito")]
    public bool? aceito { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:dataValidade")]
    public string? dataValidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valorMensal")]
    public double? valorMensal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:comArquivo")]
    public bool? comArquivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:comProduto")]
    public bool? comProduto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:comProdutoTerceiro")]
    public bool? comProdutoTerceiro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:comServico")]
    public bool? comServico { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valorDesconto")]
    public double? valorDesconto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valorAcrescimo")]
    public double? valorAcrescimo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valorFrete")]
    public double? valorFrete { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:informacoes")]
    public string? informacoes { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:descontoPercentual")]
    public double? descontoPercentual { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valorItens")]
    public double? valorItens { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:idCondicaoPagamento")]
    public int? idCondicaoPagamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:dataPrevistaEntrega")]
    public string? dataPrevistaEntrega { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:moeda")]
    public string? moeda { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valorConversao")]
    public double? valorConversao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:imprimeMoedaAdd")]
    public string? imprimeMoedaAdd { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valorDescontoMensal")]
    public double? valorDescontoMensal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valorAcrescimoMensal")]
    public double? valorAcrescimoMensal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valorFreteMensal")]
    public double? valorFreteMensal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:descontoPercentualMensal")]
    public double? descontoPercentualMensal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOrcamentos:valorItensMensal")]
    public double? valorItensMensal { get; set; }
}
