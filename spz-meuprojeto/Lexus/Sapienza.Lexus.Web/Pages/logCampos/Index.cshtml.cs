using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.logCampos;
using Sapienza.Lexus.logCampos.Dtos;

namespace Sapienza.Lexus.Web.Pages.logCampos;

public class IndexModel : Sapienza.LexusPageModel
{
    public logCamposFilterInput logCamposFilter { get; set; }
    
    private readonly IlogCamposAppService _logCamposAppService;

    public IndexModel(IlogCamposAppService logCamposAppService)
    {
        _logCamposAppService = logCamposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(logCamposGetListInput input)
    {
        var result = await _logCamposAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _logCamposAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class logCamposFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logCampos:idLogCampo")]
    public int? idLogCampo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logCampos:idLog")]
    public int? idLog { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logCampos:campo")]
    public string? campo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logCampos:dadoAnterior")]
    public string? dadoAnterior { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logCampos:dadoNovo")]
    public string? dadoNovo { get; set; }
}
