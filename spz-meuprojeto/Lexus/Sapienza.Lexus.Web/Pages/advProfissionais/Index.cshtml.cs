using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProfissionais;
using Sapienza.Lexus.advProfissionais.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProfissionais;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProfissionaisFilterInput advProfissionaisFilter { get; set; }
    
    private readonly IadvProfissionaisAppService _advProfissionaisAppService;

    public IndexModel(IadvProfissionaisAppService advProfissionaisAppService)
    {
        _advProfissionaisAppService = advProfissionaisAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProfissionaisGetListInput input)
    {
        var result = await _advProfissionaisAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProfissionaisAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProfissionaisFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionais:idProfissional")]
    public int? idProfissional { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionais:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionais:nome")]
    public string? nome { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionais:email")]
    public string? email { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionais:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionais:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProfissionais:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
