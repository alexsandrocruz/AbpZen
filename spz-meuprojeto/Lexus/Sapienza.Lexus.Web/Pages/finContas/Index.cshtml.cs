using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finContas;
using Sapienza.Lexus.finContas.Dtos;

namespace Sapienza.Lexus.Web.Pages.finContas;

public class IndexModel : Sapienza.LexusPageModel
{
    public finContasFilterInput finContasFilter { get; set; }
    
    private readonly IfinContasAppService _finContasAppService;

    public IndexModel(IfinContasAppService finContasAppService)
    {
        _finContasAppService = finContasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finContasGetListInput input)
    {
        var result = await _finContasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finContasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finContasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:idConta")]
    public int? idConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:banco")]
    public string? banco { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:agencia")]
    public string? agencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:conta")]
    public string? conta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:favorecido")]
    public string? favorecido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:limite")]
    public double? limite { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:padraoFluxo")]
    public bool? padraoFluxo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:considerarIndicador")]
    public bool? considerarIndicador { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:saldoInicial")]
    public double? saldoInicial { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:padrao")]
    public bool? padrao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:codigo")]
    public string? codigo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContas:cor")]
    public string? cor { get; set; }
}
