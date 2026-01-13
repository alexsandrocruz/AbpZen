using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus._versaoBD;
using Sapienza.Lexus._versaoBD.Dtos;

namespace Sapienza.Lexus.Web.Pages._versaoBD;

public class IndexModel : Sapienza.LexusPageModel
{
    public _versaoBDFilterInput _versaoBDFilter { get; set; }
    
    private readonly I_versaoBDAppService __versaoBDAppService;

    public IndexModel(I_versaoBDAppService _versaoBDAppService)
    {
        __versaoBDAppService = _versaoBDAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(_versaoBDGetListInput input)
    {
        var result = await __versaoBDAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await __versaoBDAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class _versaoBDFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "_versaoBD:id")]
    public int? id { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "_versaoBD:arquivo")]
    public string? arquivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "_versaoBD:dataAplicacao")]
    public DateTime? dataAplicacao { get; set; }
}
