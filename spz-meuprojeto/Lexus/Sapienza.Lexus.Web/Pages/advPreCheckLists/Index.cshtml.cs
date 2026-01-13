using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPreCheckLists;
using Sapienza.Lexus.advPreCheckLists.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPreCheckLists;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPreCheckListsFilterInput advPreCheckListsFilter { get; set; }
    
    private readonly IadvPreCheckListsAppService _advPreCheckListsAppService;

    public IndexModel(IadvPreCheckListsAppService advPreCheckListsAppService)
    {
        _advPreCheckListsAppService = advPreCheckListsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPreCheckListsGetListInput input)
    {
        var result = await _advPreCheckListsAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPreCheckListsAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPreCheckListsFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckLists:idCheckList")]
    public int? idCheckList { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckLists:idGrupo")]
    public int? idGrupo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckLists:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckLists:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckLists:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreCheckLists:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
