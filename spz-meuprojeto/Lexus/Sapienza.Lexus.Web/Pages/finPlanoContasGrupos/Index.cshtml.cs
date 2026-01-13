using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finPlanoContasGrupos;
using Sapienza.Lexus.finPlanoContasGrupos.Dtos;

namespace Sapienza.Lexus.Web.Pages.finPlanoContasGrupos;

public class IndexModel : Sapienza.LexusPageModel
{
    public finPlanoContasGruposFilterInput finPlanoContasGruposFilter { get; set; }
    
    private readonly IfinPlanoContasGruposAppService _finPlanoContasGruposAppService;

    public IndexModel(IfinPlanoContasGruposAppService finPlanoContasGruposAppService)
    {
        _finPlanoContasGruposAppService = finPlanoContasGruposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finPlanoContasGruposGetListInput input)
    {
        var result = await _finPlanoContasGruposAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finPlanoContasGruposAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finPlanoContasGruposFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasGrupos:idGrupo")]
    public int? idGrupo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasGrupos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasGrupos:tipo")]
    public string? tipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasGrupos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasGrupos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasGrupos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasGrupos:idGrupoDRE")]
    public int? idGrupoDRE { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasGrupos:ordem")]
    public int? ordem { get; set; }
}
