using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPreOrigens;
using Sapienza.Lexus.advPreOrigens.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPreOrigens;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPreOrigensFilterInput advPreOrigensFilter { get; set; }
    
    private readonly IadvPreOrigensAppService _advPreOrigensAppService;

    public IndexModel(IadvPreOrigensAppService advPreOrigensAppService)
    {
        _advPreOrigensAppService = advPreOrigensAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPreOrigensGetListInput input)
    {
        var result = await _advPreOrigensAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPreOrigensAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPreOrigensFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreOrigens:idOrigem")]
    public int? idOrigem { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreOrigens:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreOrigens:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreOrigens:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreOrigens:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
