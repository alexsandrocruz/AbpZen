using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.flwConfig;
using Sapienza.Lexus.flwConfig.Dtos;

namespace Sapienza.Lexus.Web.Pages.flwConfig;

public class IndexModel : Sapienza.LexusPageModel
{
    public flwConfigFilterInput flwConfigFilter { get; set; }
    
    private readonly IflwConfigAppService _flwConfigAppService;

    public IndexModel(IflwConfigAppService flwConfigAppService)
    {
        _flwConfigAppService = flwConfigAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(flwConfigGetListInput input)
    {
        var result = await _flwConfigAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _flwConfigAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class flwConfigFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwConfig:idConfig")]
    public int? idConfig { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwConfig:tipoMarcacoes")]
    public string? tipoMarcacoes { get; set; }
}
