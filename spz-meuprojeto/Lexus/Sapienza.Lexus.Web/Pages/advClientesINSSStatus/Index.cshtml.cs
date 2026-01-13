using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advClientesINSSStatus;
using Sapienza.Lexus.advClientesINSSStatus.Dtos;

namespace Sapienza.Lexus.Web.Pages.advClientesINSSStatus;

public class IndexModel : Sapienza.LexusPageModel
{
    public advClientesINSSStatusFilterInput advClientesINSSStatusFilter { get; set; }
    
    private readonly IadvClientesINSSStatusAppService _advClientesINSSStatusAppService;

    public IndexModel(IadvClientesINSSStatusAppService advClientesINSSStatusAppService)
    {
        _advClientesINSSStatusAppService = advClientesINSSStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advClientesINSSStatusGetListInput input)
    {
        var result = await _advClientesINSSStatusAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advClientesINSSStatusAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advClientesINSSStatusFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSSStatus:idStatus")]
    public int? idStatus { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSSStatus:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSSStatus:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSSStatus:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSSStatus:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
