using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProcessosMeritos;
using Sapienza.Lexus.advProcessosMeritos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProcessosMeritos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProcessosMeritosFilterInput advProcessosMeritosFilter { get; set; }
    
    private readonly IadvProcessosMeritosAppService _advProcessosMeritosAppService;

    public IndexModel(IadvProcessosMeritosAppService advProcessosMeritosAppService)
    {
        _advProcessosMeritosAppService = advProcessosMeritosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProcessosMeritosGetListInput input)
    {
        var result = await _advProcessosMeritosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProcessosMeritosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProcessosMeritosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosMeritos:idProcessoMerito")]
    public int? idProcessoMerito { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosMeritos:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosMeritos:idMerito")]
    public int? idMerito { get; set; }
}
