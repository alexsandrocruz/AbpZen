using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.Lawyer;
using Sapienza.Lexus.Lawyer.Dtos;

namespace Sapienza.Lexus.Web.Pages.Lawyer;

public class IndexModel : Sapienza.LexusPageModel
{
    public LawyerFilterInput LawyerFilter { get; set; }
    
    private readonly ILawyerAppService _lawyerAppService;

    public IndexModel(ILawyerAppService lawyerAppService)
    {
        _lawyerAppService = lawyerAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(LawyerGetListInput input)
    {
        var result = await _lawyerAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _lawyerAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class LawyerFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Lawyer:FullName")]
    public string? FullName { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Lawyer:PreferredName")]
    public string? PreferredName { get; set; }
}
