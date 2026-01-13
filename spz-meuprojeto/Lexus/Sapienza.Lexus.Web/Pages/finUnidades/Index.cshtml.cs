using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finUnidades;
using Sapienza.Lexus.finUnidades.Dtos;

namespace Sapienza.Lexus.Web.Pages.finUnidades;

public class IndexModel : Sapienza.LexusPageModel
{
    public finUnidadesFilterInput finUnidadesFilter { get; set; }
    
    private readonly IfinUnidadesAppService _finUnidadesAppService;

    public IndexModel(IfinUnidadesAppService finUnidadesAppService)
    {
        _finUnidadesAppService = finUnidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finUnidadesGetListInput input)
    {
        var result = await _finUnidadesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finUnidadesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finUnidadesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finUnidades:idUnidade")]
    public int? idUnidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finUnidades:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finUnidades:percentual")]
    public double? percentual { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finUnidades:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finUnidades:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finUnidades:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
