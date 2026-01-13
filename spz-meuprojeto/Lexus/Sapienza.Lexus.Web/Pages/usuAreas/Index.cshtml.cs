using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.usuAreas;
using Sapienza.Lexus.usuAreas.Dtos;

namespace Sapienza.Lexus.Web.Pages.usuAreas;

public class IndexModel : Sapienza.LexusPageModel
{
    public usuAreasFilterInput usuAreasFilter { get; set; }
    
    private readonly IusuAreasAppService _usuAreasAppService;

    public IndexModel(IusuAreasAppService usuAreasAppService)
    {
        _usuAreasAppService = usuAreasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(usuAreasGetListInput input)
    {
        var result = await _usuAreasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _usuAreasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class usuAreasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuAreas:idArea")]
    public int? idArea { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuAreas:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuAreas:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuAreas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuAreas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
