using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finRecibos;
using Sapienza.Lexus.finRecibos.Dtos;

namespace Sapienza.Lexus.Web.Pages.finRecibos;

public class IndexModel : Sapienza.LexusPageModel
{
    public finRecibosFilterInput finRecibosFilter { get; set; }
    
    private readonly IfinRecibosAppService _finRecibosAppService;

    public IndexModel(IfinRecibosAppService finRecibosAppService)
    {
        _finRecibosAppService = finRecibosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finRecibosGetListInput input)
    {
        var result = await _finRecibosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finRecibosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finRecibosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRecibos:idRecibo")]
    public int? idRecibo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRecibos:idLancamento")]
    public int? idLancamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRecibos:numero")]
    public int? numero { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRecibos:referente")]
    public string? referente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRecibos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRecibos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finRecibos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
