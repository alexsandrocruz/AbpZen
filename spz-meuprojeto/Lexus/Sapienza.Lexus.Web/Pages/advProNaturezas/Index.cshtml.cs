using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProNaturezas;
using Sapienza.Lexus.advProNaturezas.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProNaturezas;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProNaturezasFilterInput advProNaturezasFilter { get; set; }
    
    private readonly IadvProNaturezasAppService _advProNaturezasAppService;

    public IndexModel(IadvProNaturezasAppService advProNaturezasAppService)
    {
        _advProNaturezasAppService = advProNaturezasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProNaturezasGetListInput input)
    {
        var result = await _advProNaturezasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProNaturezasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProNaturezasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:idNatureza")]
    public int? idNatureza { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:mostraHistoricoNumeros")]
    public bool? mostraHistoricoNumeros { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:recebeAcordo")]
    public bool? recebeAcordo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:recebeRPV")]
    public bool? recebeRPV { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:recebePrecatorio")]
    public bool? recebePrecatorio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:recebeAlvara")]
    public bool? recebeAlvara { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProNaturezas:idArea")]
    public int? idArea { get; set; }
}
