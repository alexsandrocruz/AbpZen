using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProTipos;
using Sapienza.Lexus.advProTipos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProTipos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProTiposFilterInput advProTiposFilter { get; set; }
    
    private readonly IadvProTiposAppService _advProTiposAppService;

    public IndexModel(IadvProTiposAppService advProTiposAppService)
    {
        _advProTiposAppService = advProTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProTiposGetListInput input)
    {
        var result = await _advProTiposAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProTiposAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProTiposFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProTipos:idTipo")]
    public int? idTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProTipos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProTipos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProTipos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProTipos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
