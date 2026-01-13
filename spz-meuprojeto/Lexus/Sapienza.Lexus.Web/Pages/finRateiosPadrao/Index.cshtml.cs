using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finRateiosPadrao;
using Sapienza.Lexus.finRateiosPadrao.Dtos;

namespace Sapienza.Lexus.Web.Pages.finRateiosPadrao;

public class IndexModel : Sapienza.LexusPageModel
{
    public finRateiosPadraoFilterInput finRateiosPadraoFilter { get; set; }
    
    private readonly IfinRateiosPadraoAppService _finRateiosPadraoAppService;

    public IndexModel(IfinRateiosPadraoAppService finRateiosPadraoAppService)
    {
        _finRateiosPadraoAppService = finRateiosPadraoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finRateiosPadraoGetListInput input)
    {
        var result = await _finRateiosPadraoAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finRateiosPadraoAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finRateiosPadraoFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateiosPadrao:idPadrao")]
    public int? idPadrao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateiosPadrao:idUnidade")]
    public int? idUnidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateiosPadrao:idCentroResultado")]
    public int? idCentroResultado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateiosPadrao:porcentagem")]
    public double? porcentagem { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateiosPadrao:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateiosPadrao:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRateiosPadrao:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
