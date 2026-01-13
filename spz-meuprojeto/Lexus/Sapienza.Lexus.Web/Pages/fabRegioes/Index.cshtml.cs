using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabRegioes;
using Sapienza.Lexus.fabRegioes.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabRegioes;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabRegioesFilterInput fabRegioesFilter { get; set; }
    
    private readonly IfabRegioesAppService _fabRegioesAppService;

    public IndexModel(IfabRegioesAppService fabRegioesAppService)
    {
        _fabRegioesAppService = fabRegioesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabRegioesGetListInput input)
    {
        var result = await _fabRegioesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabRegioesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabRegioesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabRegioes:idRegiao")]
    public int? idRegiao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabRegioes:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabRegioes:estados")]
    public string? estados { get; set; }
}
