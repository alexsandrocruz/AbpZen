using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPreStatus;
using Sapienza.Lexus.advPreStatus.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPreStatus;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPreStatusFilterInput advPreStatusFilter { get; set; }
    
    private readonly IadvPreStatusAppService _advPreStatusAppService;

    public IndexModel(IadvPreStatusAppService advPreStatusAppService)
    {
        _advPreStatusAppService = advPreStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPreStatusGetListInput input)
    {
        var result = await _advPreStatusAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPreStatusAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPreStatusFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatus:idStatus")]
    public int? idStatus { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatus:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatus:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatus:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatus:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatus:ordem")]
    public int? ordem { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatus:ultimo")]
    public bool? ultimo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatus:diasMaxParado")]
    public int? diasMaxParado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatus:idTipo")]
    public int? idTipo { get; set; }
}
