using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCliComoChegou;
using Sapienza.Lexus.advCliComoChegou.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCliComoChegou;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCliComoChegouFilterInput advCliComoChegouFilter { get; set; }
    
    private readonly IadvCliComoChegouAppService _advCliComoChegouAppService;

    public IndexModel(IadvCliComoChegouAppService advCliComoChegouAppService)
    {
        _advCliComoChegouAppService = advCliComoChegouAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCliComoChegouGetListInput input)
    {
        var result = await _advCliComoChegouAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCliComoChegouAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCliComoChegouFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliComoChegou:idComoChegou")]
    public int? idComoChegou { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliComoChegou:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliComoChegou:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliComoChegou:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliComoChegou:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
