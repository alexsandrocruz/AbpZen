using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProfissionaisNaturezas;
using Sapienza.Lexus.advProfissionaisNaturezas.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProfissionaisNaturezas;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProfissionaisNaturezasFilterInput advProfissionaisNaturezasFilter { get; set; }
    
    private readonly IadvProfissionaisNaturezasAppService _advProfissionaisNaturezasAppService;

    public IndexModel(IadvProfissionaisNaturezasAppService advProfissionaisNaturezasAppService)
    {
        _advProfissionaisNaturezasAppService = advProfissionaisNaturezasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProfissionaisNaturezasGetListInput input)
    {
        var result = await _advProfissionaisNaturezasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProfissionaisNaturezasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProfissionaisNaturezasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionaisNaturezas:idProfissionalNatureza")]
    public int? idProfissionalNatureza { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionaisNaturezas:idProfissional")]
    public int? idProfissional { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionaisNaturezas:idNatureza")]
    public int? idNatureza { get; set; }
}
