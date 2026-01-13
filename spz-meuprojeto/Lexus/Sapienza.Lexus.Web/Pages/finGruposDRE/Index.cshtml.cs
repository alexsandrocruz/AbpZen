using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finGruposDRE;
using Sapienza.Lexus.finGruposDRE.Dtos;

namespace Sapienza.Lexus.Web.Pages.finGruposDRE;

public class IndexModel : Sapienza.LexusPageModel
{
    public finGruposDREFilterInput finGruposDREFilter { get; set; }
    
    private readonly IfinGruposDREAppService _finGruposDREAppService;

    public IndexModel(IfinGruposDREAppService finGruposDREAppService)
    {
        _finGruposDREAppService = finGruposDREAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finGruposDREGetListInput input)
    {
        var result = await _finGruposDREAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finGruposDREAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finGruposDREFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finGruposDRE:idGrupoDRE")]
    public int? idGrupoDRE { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finGruposDRE:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finGruposDRE:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finGruposDRE:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finGruposDRE:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
