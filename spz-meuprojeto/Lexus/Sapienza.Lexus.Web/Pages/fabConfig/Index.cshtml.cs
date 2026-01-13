using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabConfig;
using Sapienza.Lexus.fabConfig.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabConfig;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabConfigFilterInput fabConfigFilter { get; set; }
    
    private readonly IfabConfigAppService _fabConfigAppService;

    public IndexModel(IfabConfigAppService fabConfigAppService)
    {
        _fabConfigAppService = fabConfigAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabConfigGetListInput input)
    {
        var result = await _fabConfigAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabConfigAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabConfigFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabConfig:idConfig")]
    public int? idConfig { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabConfig:imagemLogin")]
    public string? imagemLogin { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabConfig:imagemLoginCentral")]
    public string? imagemLoginCentral { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabConfig:imagemLoginTickets")]
    public string? imagemLoginTickets { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabConfig:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabConfig:precoCombustivel")]
    public double? precoCombustivel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabConfig:dataBloqueioFinanceiro")]
    public string? dataBloqueioFinanceiro { get; set; }
}
