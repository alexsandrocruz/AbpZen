using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabPaises;
using Sapienza.Lexus.fabPaises.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabPaises;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabPaisesFilterInput fabPaisesFilter { get; set; }
    
    private readonly IfabPaisesAppService _fabPaisesAppService;

    public IndexModel(IfabPaisesAppService fabPaisesAppService)
    {
        _fabPaisesAppService = fabPaisesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabPaisesGetListInput input)
    {
        var result = await _fabPaisesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabPaisesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabPaisesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPaises:idPais")]
    public int? idPais { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPaises:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPaises:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPaises:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPaises:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
