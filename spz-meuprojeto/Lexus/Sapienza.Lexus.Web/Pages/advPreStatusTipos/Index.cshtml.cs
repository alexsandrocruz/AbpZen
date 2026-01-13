using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPreStatusTipos;
using Sapienza.Lexus.advPreStatusTipos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPreStatusTipos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPreStatusTiposFilterInput advPreStatusTiposFilter { get; set; }
    
    private readonly IadvPreStatusTiposAppService _advPreStatusTiposAppService;

    public IndexModel(IadvPreStatusTiposAppService advPreStatusTiposAppService)
    {
        _advPreStatusTiposAppService = advPreStatusTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPreStatusTiposGetListInput input)
    {
        var result = await _advPreStatusTiposAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPreStatusTiposAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPreStatusTiposFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatusTipos:idTipo")]
    public int? idTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatusTipos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatusTipos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatusTipos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreStatusTipos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
