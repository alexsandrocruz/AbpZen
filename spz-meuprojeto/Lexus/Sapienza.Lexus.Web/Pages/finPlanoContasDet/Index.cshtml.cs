using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finPlanoContasDet;
using Sapienza.Lexus.finPlanoContasDet.Dtos;

namespace Sapienza.Lexus.Web.Pages.finPlanoContasDet;

public class IndexModel : Sapienza.LexusPageModel
{
    public finPlanoContasDetFilterInput finPlanoContasDetFilter { get; set; }
    
    private readonly IfinPlanoContasDetAppService _finPlanoContasDetAppService;

    public IndexModel(IfinPlanoContasDetAppService finPlanoContasDetAppService)
    {
        _finPlanoContasDetAppService = finPlanoContasDetAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finPlanoContasDetGetListInput input)
    {
        var result = await _finPlanoContasDetAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finPlanoContasDetAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finPlanoContasDetFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasDet:idPlanoContasDet")]
    public int? idPlanoContasDet { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasDet:idPlanoConta")]
    public int? idPlanoConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasDet:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasDet:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasDet:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finPlanoContasDet:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
