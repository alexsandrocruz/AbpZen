using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finPrestacaoContas;
using Sapienza.Lexus.finPrestacaoContas.Dtos;

namespace Sapienza.Lexus.Web.Pages.finPrestacaoContas;

public class IndexModel : Sapienza.LexusPageModel
{
    public finPrestacaoContasFilterInput finPrestacaoContasFilter { get; set; }
    
    private readonly IfinPrestacaoContasAppService _finPrestacaoContasAppService;

    public IndexModel(IfinPrestacaoContasAppService finPrestacaoContasAppService)
    {
        _finPrestacaoContasAppService = finPrestacaoContasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finPrestacaoContasGetListInput input)
    {
        var result = await _finPrestacaoContasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finPrestacaoContasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finPrestacaoContasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPrestacaoContas:idPrestacao")]
    public int? idPrestacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPrestacaoContas:idLancamento")]
    public int? idLancamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPrestacaoContas:levantado")]
    public double? levantado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPrestacaoContas:irpj")]
    public double? irpj { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPrestacaoContas:carta")]
    public double? carta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPrestacaoContas:honorarios")]
    public double? honorarios { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPrestacaoContas:tarifa")]
    public double? tarifa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPrestacaoContas:liquidoRecebido")]
    public double? liquidoRecebido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPrestacaoContas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
}
