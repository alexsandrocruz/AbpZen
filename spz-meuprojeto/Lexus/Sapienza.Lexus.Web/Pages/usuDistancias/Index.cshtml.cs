using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.usuDistancias;
using Sapienza.Lexus.usuDistancias.Dtos;

namespace Sapienza.Lexus.Web.Pages.usuDistancias;

public class IndexModel : Sapienza.LexusPageModel
{
    public usuDistanciasFilterInput usuDistanciasFilter { get; set; }
    
    private readonly IusuDistanciasAppService _usuDistanciasAppService;

    public IndexModel(IusuDistanciasAppService usuDistanciasAppService)
    {
        _usuDistanciasAppService = usuDistanciasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(usuDistanciasGetListInput input)
    {
        var result = await _usuDistanciasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _usuDistanciasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class usuDistanciasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuDistancias:idDistancia")]
    public int? idDistancia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuDistancias:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuDistancias:estado")]
    public string? estado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuDistancias:cidade")]
    public string? cidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuDistancias:km")]
    public int? km { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuDistancias:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
}
