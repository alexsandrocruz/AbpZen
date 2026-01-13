using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCliGrupos;
using Sapienza.Lexus.advCliGrupos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCliGrupos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCliGruposFilterInput advCliGruposFilter { get; set; }
    
    private readonly IadvCliGruposAppService _advCliGruposAppService;

    public IndexModel(IadvCliGruposAppService advCliGruposAppService)
    {
        _advCliGruposAppService = advCliGruposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCliGruposGetListInput input)
    {
        var result = await _advCliGruposAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCliGruposAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCliGruposFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliGrupos:idGrupo")]
    public int? idGrupo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliGrupos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliGrupos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliGrupos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliGrupos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
