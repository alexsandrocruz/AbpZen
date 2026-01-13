using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProRelevancias;
using Sapienza.Lexus.advProRelevancias.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProRelevancias;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProRelevanciasFilterInput advProRelevanciasFilter { get; set; }
    
    private readonly IadvProRelevanciasAppService _advProRelevanciasAppService;

    public IndexModel(IadvProRelevanciasAppService advProRelevanciasAppService)
    {
        _advProRelevanciasAppService = advProRelevanciasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProRelevanciasGetListInput input)
    {
        var result = await _advProRelevanciasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProRelevanciasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProRelevanciasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProRelevancias:idRelevancia")]
    public int? idRelevancia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProRelevancias:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProRelevancias:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProRelevancias:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProRelevancias:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
