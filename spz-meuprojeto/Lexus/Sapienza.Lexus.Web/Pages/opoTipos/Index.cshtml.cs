using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.opoTipos;
using Sapienza.Lexus.opoTipos.Dtos;

namespace Sapienza.Lexus.Web.Pages.opoTipos;

public class IndexModel : Sapienza.LexusPageModel
{
    public opoTiposFilterInput opoTiposFilter { get; set; }
    
    private readonly IopoTiposAppService _opoTiposAppService;

    public IndexModel(IopoTiposAppService opoTiposAppService)
    {
        _opoTiposAppService = opoTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(opoTiposGetListInput input)
    {
        var result = await _opoTiposAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _opoTiposAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class opoTiposFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoTipos:idTipo")]
    public int? idTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoTipos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoTipos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoTipos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoTipos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
