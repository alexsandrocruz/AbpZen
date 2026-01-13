using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finRateios;
using Sapienza.Lexus.finRateios.Dtos;

namespace Sapienza.Lexus.Web.Pages.finRateios;

public class IndexModel : Sapienza.LexusPageModel
{
    public finRateiosFilterInput finRateiosFilter { get; set; }
    
    private readonly IfinRateiosAppService _finRateiosAppService;

    public IndexModel(IfinRateiosAppService finRateiosAppService)
    {
        _finRateiosAppService = finRateiosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finRateiosGetListInput input)
    {
        var result = await _finRateiosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finRateiosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finRateiosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateios:idRateio")]
    public int? idRateio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateios:idLancamento")]
    public int? idLancamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateios:idCentroCusto")]
    public int? idCentroCusto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateios:idCentroResultado")]
    public int? idCentroResultado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateios:percentualCC")]
    public double? percentualCC { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateios:percentualCR")]
    public double? percentualCR { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateios:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateios:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateios:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateios:idUnidade")]
    public int? idUnidade { get; set; }
}
