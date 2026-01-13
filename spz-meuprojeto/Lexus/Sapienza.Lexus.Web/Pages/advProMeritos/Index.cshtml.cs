using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProMeritos;
using Sapienza.Lexus.advProMeritos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProMeritos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProMeritosFilterInput advProMeritosFilter { get; set; }
    
    private readonly IadvProMeritosAppService _advProMeritosAppService;

    public IndexModel(IadvProMeritosAppService advProMeritosAppService)
    {
        _advProMeritosAppService = advProMeritosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProMeritosGetListInput input)
    {
        var result = await _advProMeritosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProMeritosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProMeritosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProMeritos:idMerito")]
    public int? idMerito { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProMeritos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProMeritos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProMeritos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProMeritos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProMeritos:beneficioINSS")]
    public bool? beneficioINSS { get; set; }
}
