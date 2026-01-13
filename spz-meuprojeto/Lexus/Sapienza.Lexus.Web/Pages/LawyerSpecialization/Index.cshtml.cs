using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.LawyerSpecialization;
using Sapienza.Lexus.LawyerSpecialization.Dtos;

namespace Sapienza.Lexus.Web.Pages.LawyerSpecialization;

public class IndexModel : Sapienza.LexusPageModel
{
    public LawyerSpecializationFilterInput LawyerSpecializationFilter { get; set; }
    
    private readonly ILawyerSpecializationAppService _lawyerSpecializationAppService;

    public IndexModel(ILawyerSpecializationAppService lawyerSpecializationAppService)
    {
        _lawyerSpecializationAppService = lawyerSpecializationAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(LawyerSpecializationGetListInput input)
    {
        var result = await _lawyerSpecializationAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _lawyerSpecializationAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class LawyerSpecializationFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LawyerSpecialization:LawyerId")]
    public Guid? LawyerId { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LawyerSpecialization:SpecializationId")]
    public Guid? SpecializationId { get; set; }
}
