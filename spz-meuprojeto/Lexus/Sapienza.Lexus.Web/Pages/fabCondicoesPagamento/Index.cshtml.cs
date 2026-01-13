using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabCondicoesPagamento;
using Sapienza.Lexus.fabCondicoesPagamento.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabCondicoesPagamento;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabCondicoesPagamentoFilterInput fabCondicoesPagamentoFilter { get; set; }
    
    private readonly IfabCondicoesPagamentoAppService _fabCondicoesPagamentoAppService;

    public IndexModel(IfabCondicoesPagamentoAppService fabCondicoesPagamentoAppService)
    {
        _fabCondicoesPagamentoAppService = fabCondicoesPagamentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabCondicoesPagamentoGetListInput input)
    {
        var result = await _fabCondicoesPagamentoAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabCondicoesPagamentoAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabCondicoesPagamentoFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:idCondicaoPagamento")]
    public int? idCondicaoPagamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:parcelas")]
    public int? parcelas { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p1")]
    public double? p1 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d1")]
    public int? d1 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p2")]
    public double? p2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d2")]
    public int? d2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p3")]
    public double? p3 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d3")]
    public int? d3 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p4")]
    public double? p4 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d4")]
    public int? d4 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p5")]
    public double? p5 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d5")]
    public int? d5 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p6")]
    public double? p6 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d6")]
    public int? d6 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p7")]
    public double? p7 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d7")]
    public int? d7 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p8")]
    public double? p8 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d8")]
    public int? d8 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p9")]
    public double? p9 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d9")]
    public int? d9 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p10")]
    public double? p10 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d10")]
    public int? d10 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p11")]
    public double? p11 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d11")]
    public int? d11 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:p12")]
    public double? p12 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:d12")]
    public int? d12 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:compras")]
    public bool? compras { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:vendas")]
    public bool? vendas { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:valorMinimo")]
    public double? valorMinimo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabCondicoesPagamento:atendimento")]
    public bool? atendimento { get; set; }
}
