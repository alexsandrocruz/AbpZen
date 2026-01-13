using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPreProcessosCheckLists;
using Sapienza.Lexus.advPreProcessosCheckLists.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPreProcessosCheckLists;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPreProcessosCheckListsFilterInput advPreProcessosCheckListsFilter { get; set; }
    
    private readonly IadvPreProcessosCheckListsAppService _advPreProcessosCheckListsAppService;

    public IndexModel(IadvPreProcessosCheckListsAppService advPreProcessosCheckListsAppService)
    {
        _advPreProcessosCheckListsAppService = advPreProcessosCheckListsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPreProcessosCheckListsGetListInput input)
    {
        var result = await _advPreProcessosCheckListsAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPreProcessosCheckListsAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPreProcessosCheckListsFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:idPreCheckList")]
    public int? idPreCheckList { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:idGrupo")]
    public int? idGrupo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:idCheckList")]
    public int? idCheckList { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:grupo")]
    public string? grupo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:item")]
    public string? item { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:concluido")]
    public bool? concluido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:tsConclusao")]
    public string? tsConclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:idResponsavel")]
    public int? idResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreProcessosCheckLists:ordem")]
    public int? ordem { get; set; }
}
