using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finAreas;
using Sapienza.Lexus.finAreas.Dtos;

namespace Sapienza.Lexus.Web.Pages.finAreas;

public class IndexModel : Sapienza.LexusPageModel
{
    public finAreasFilterInput finAreasFilter { get; set; }
    
    private readonly IfinAreasAppService _finAreasAppService;

    public IndexModel(IfinAreasAppService finAreasAppService)
    {
        _finAreasAppService = finAreasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finAreasGetListInput input)
    {
        var result = await _finAreasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finAreasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finAreasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finAreas:idArea")]
    public int? idArea { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finAreas:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finAreas:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finAreas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finAreas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finAreas:idCentroResultado")]
    public int? idCentroResultado { get; set; }
}
