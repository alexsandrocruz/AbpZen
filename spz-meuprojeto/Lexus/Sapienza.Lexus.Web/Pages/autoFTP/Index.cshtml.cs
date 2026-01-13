using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.autoFTP;
using Sapienza.Lexus.autoFTP.Dtos;

namespace Sapienza.Lexus.Web.Pages.autoFTP;

public class IndexModel : Sapienza.LexusPageModel
{
    public autoFTPFilterInput autoFTPFilter { get; set; }
    
    private readonly IautoFTPAppService _autoFTPAppService;

    public IndexModel(IautoFTPAppService autoFTPAppService)
    {
        _autoFTPAppService = autoFTPAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(autoFTPGetListInput input)
    {
        var result = await _autoFTPAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _autoFTPAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class autoFTPFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "autoFTP:id")]
    public int? id { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "autoFTP:arquivo")]
    public string? arquivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "autoFTP:processado")]
    public bool? processado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "autoFTP:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
}
