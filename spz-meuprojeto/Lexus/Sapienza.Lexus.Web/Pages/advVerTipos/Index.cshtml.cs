using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advVerTipos;
using Sapienza.Lexus.advVerTipos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advVerTipos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advVerTiposFilterInput advVerTiposFilter { get; set; }
    
    private readonly IadvVerTiposAppService _advVerTiposAppService;

    public IndexModel(IadvVerTiposAppService advVerTiposAppService)
    {
        _advVerTiposAppService = advVerTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advVerTiposGetListInput input)
    {
        var result = await _advVerTiposAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advVerTiposAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advVerTiposFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerTipos:idTipo")]
    public int? idTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerTipos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerTipos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerTipos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerTipos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
