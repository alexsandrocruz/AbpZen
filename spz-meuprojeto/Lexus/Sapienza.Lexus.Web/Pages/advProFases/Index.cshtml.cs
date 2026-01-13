using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProFases;
using Sapienza.Lexus.advProFases.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProFases;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProFasesFilterInput advProFasesFilter { get; set; }
    
    private readonly IadvProFasesAppService _advProFasesAppService;

    public IndexModel(IadvProFasesAppService advProFasesAppService)
    {
        _advProFasesAppService = advProFasesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProFasesGetListInput input)
    {
        var result = await _advProFasesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProFasesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProFasesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProFases:idFase")]
    public int? idFase { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProFases:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProFases:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProFases:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProFases:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
