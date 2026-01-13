using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.LegalProcess;
using Sapienza.Lexus.LegalProcess.Dtos;

namespace Sapienza.Lexus.Web.Pages.LegalProcess;

public class IndexModel : Sapienza.LexusPageModel
{
    public LegalProcessFilterInput LegalProcessFilter { get; set; }
    
    private readonly ILegalProcessAppService _legalProcessAppService;

    public IndexModel(ILegalProcessAppService legalProcessAppService)
    {
        _legalProcessAppService = legalProcessAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(LegalProcessGetListInput input)
    {
        var result = await _legalProcessAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _legalProcessAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class LegalProcessFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LegalProcess:ProcessNumber")]
    public string? ProcessNumber { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LegalProcess:Title")]
    public string? Title { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LegalProcess:DateOpened")]
    public DateTime? DateOpened { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LegalProcess:LawyerId")]
    public Guid? LawyerId { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "LegalProcess:ClientId")]
    public Guid? ClientId { get; set; }
}
