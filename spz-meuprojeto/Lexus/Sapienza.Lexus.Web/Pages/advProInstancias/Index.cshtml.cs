using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProInstancias;
using Sapienza.Lexus.advProInstancias.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProInstancias;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProInstanciasFilterInput advProInstanciasFilter { get; set; }
    
    private readonly IadvProInstanciasAppService _advProInstanciasAppService;

    public IndexModel(IadvProInstanciasAppService advProInstanciasAppService)
    {
        _advProInstanciasAppService = advProInstanciasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProInstanciasGetListInput input)
    {
        var result = await _advProInstanciasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProInstanciasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProInstanciasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProInstancias:idInstancia")]
    public int? idInstancia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProInstancias:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProInstancias:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProInstancias:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProInstancias:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
