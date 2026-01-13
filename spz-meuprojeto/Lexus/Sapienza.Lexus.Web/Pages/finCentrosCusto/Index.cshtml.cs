using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finCentrosCusto;
using Sapienza.Lexus.finCentrosCusto.Dtos;

namespace Sapienza.Lexus.Web.Pages.finCentrosCusto;

public class IndexModel : Sapienza.LexusPageModel
{
    public finCentrosCustoFilterInput finCentrosCustoFilter { get; set; }
    
    private readonly IfinCentrosCustoAppService _finCentrosCustoAppService;

    public IndexModel(IfinCentrosCustoAppService finCentrosCustoAppService)
    {
        _finCentrosCustoAppService = finCentrosCustoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finCentrosCustoGetListInput input)
    {
        var result = await _finCentrosCustoAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finCentrosCustoAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finCentrosCustoFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosCusto:idCentroCusto")]
    public int? idCentroCusto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosCusto:idUnidade")]
    public int? idUnidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosCusto:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosCusto:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosCusto:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosCusto:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosCusto:padrao")]
    public bool? padrao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosCusto:porcentagemRateio")]
    public double? porcentagemRateio { get; set; }
}
