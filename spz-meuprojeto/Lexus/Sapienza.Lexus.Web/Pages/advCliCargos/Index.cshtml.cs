using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCliCargos;
using Sapienza.Lexus.advCliCargos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCliCargos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCliCargosFilterInput advCliCargosFilter { get; set; }
    
    private readonly IadvCliCargosAppService _advCliCargosAppService;

    public IndexModel(IadvCliCargosAppService advCliCargosAppService)
    {
        _advCliCargosAppService = advCliCargosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCliCargosGetListInput input)
    {
        var result = await _advCliCargosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCliCargosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCliCargosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliCargos:idCargo")]
    public int? idCargo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliCargos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliCargos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliCargos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliCargos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
