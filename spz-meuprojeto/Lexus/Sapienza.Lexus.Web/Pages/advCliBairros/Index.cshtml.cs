using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCliBairros;
using Sapienza.Lexus.advCliBairros.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCliBairros;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCliBairrosFilterInput advCliBairrosFilter { get; set; }
    
    private readonly IadvCliBairrosAppService _advCliBairrosAppService;

    public IndexModel(IadvCliBairrosAppService advCliBairrosAppService)
    {
        _advCliBairrosAppService = advCliBairrosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCliBairrosGetListInput input)
    {
        var result = await _advCliBairrosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCliBairrosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCliBairrosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliBairros:idBairro")]
    public int? idBairro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliBairros:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliBairros:cidade")]
    public string? cidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliBairros:estado")]
    public string? estado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliBairros:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliBairros:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliBairros:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
