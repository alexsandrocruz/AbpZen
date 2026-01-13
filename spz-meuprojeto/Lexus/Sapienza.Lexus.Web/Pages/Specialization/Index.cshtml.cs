using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.Specialization;
using Sapienza.Lexus.Specialization.Dtos;

namespace Sapienza.Lexus.Web.Pages.Specialization;

public class IndexModel : Sapienza.LexusPageModel
{
    public SpecializationFilterInput SpecializationFilter { get; set; }
    
    private readonly ISpecializationAppService _specializationAppService;

    public IndexModel(ISpecializationAppService specializationAppService)
    {
        _specializationAppService = specializationAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(SpecializationGetListInput input)
    {
        var result = await _specializationAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _specializationAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class SpecializationFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Specialization:Name")]
    public string? Name { get; set; }
}
