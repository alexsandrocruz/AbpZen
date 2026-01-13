using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPreMetas;
using Sapienza.Lexus.advPreMetas.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPreMetas;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPreMetasFilterInput advPreMetasFilter { get; set; }
    
    private readonly IadvPreMetasAppService _advPreMetasAppService;

    public IndexModel(IadvPreMetasAppService advPreMetasAppService)
    {
        _advPreMetasAppService = advPreMetasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPreMetasGetListInput input)
    {
        var result = await _advPreMetasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPreMetasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPreMetasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMetas:idMeta")]
    public int? idMeta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMetas:tipo")]
    public string? tipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMetas:idResponsavel")]
    public int? idResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMetas:idEscritorio")]
    public int? idEscritorio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMetas:qtde")]
    public int? qtde { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMetas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
}
