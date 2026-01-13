using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabFormasPagamento;
using Sapienza.Lexus.fabFormasPagamento.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabFormasPagamento;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabFormasPagamentoFilterInput fabFormasPagamentoFilter { get; set; }
    
    private readonly IfabFormasPagamentoAppService _fabFormasPagamentoAppService;

    public IndexModel(IfabFormasPagamentoAppService fabFormasPagamentoAppService)
    {
        _fabFormasPagamentoAppService = fabFormasPagamentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabFormasPagamentoGetListInput input)
    {
        var result = await _fabFormasPagamentoAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabFormasPagamentoAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabFormasPagamentoFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasPagamento:idFormaPagamento")]
    public int? idFormaPagamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasPagamento:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasPagamento:ordem")]
    public int? ordem { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasPagamento:padrao")]
    public bool? padrao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasPagamento:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasPagamento:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasPagamento:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasPagamento:idCondicaoPagamento")]
    public int? idCondicaoPagamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasPagamento:contasPagar")]
    public bool? contasPagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabFormasPagamento:compras")]
    public bool? compras { get; set; }
}
