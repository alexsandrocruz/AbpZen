using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProStatus;
using Sapienza.Lexus.advProStatus.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProStatus;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProStatusFilterInput advProStatusFilter { get; set; }
    
    private readonly IadvProStatusAppService _advProStatusAppService;

    public IndexModel(IadvProStatusAppService advProStatusAppService)
    {
        _advProStatusAppService = advProStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProStatusGetListInput input)
    {
        var result = await _advProStatusAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProStatusAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProStatusFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProStatus:idStatus")]
    public int? idStatus { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProStatus:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProStatus:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProStatus:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProStatus:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
