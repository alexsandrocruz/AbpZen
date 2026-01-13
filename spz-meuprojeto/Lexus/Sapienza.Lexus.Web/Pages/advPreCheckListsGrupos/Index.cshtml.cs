using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPreCheckListsGrupos;
using Sapienza.Lexus.advPreCheckListsGrupos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPreCheckListsGrupos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPreCheckListsGruposFilterInput advPreCheckListsGruposFilter { get; set; }
    
    private readonly IadvPreCheckListsGruposAppService _advPreCheckListsGruposAppService;

    public IndexModel(IadvPreCheckListsGruposAppService advPreCheckListsGruposAppService)
    {
        _advPreCheckListsGruposAppService = advPreCheckListsGruposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPreCheckListsGruposGetListInput input)
    {
        var result = await _advPreCheckListsGruposAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPreCheckListsGruposAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPreCheckListsGruposFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckListsGrupos:idGrupo")]
    public int? idGrupo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckListsGrupos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckListsGrupos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckListsGrupos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckListsGrupos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
