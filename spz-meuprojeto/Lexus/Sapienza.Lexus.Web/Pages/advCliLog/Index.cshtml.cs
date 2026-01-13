using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCliLog;
using Sapienza.Lexus.advCliLog.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCliLog;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCliLogFilterInput advCliLogFilter { get; set; }
    
    private readonly IadvCliLogAppService _advCliLogAppService;

    public IndexModel(IadvCliLogAppService advCliLogAppService)
    {
        _advCliLogAppService = advCliLogAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCliLogGetListInput input)
    {
        var result = await _advCliLogAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCliLogAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCliLogFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLog:idLog")]
    public int? idLog { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLog:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLog:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLog:acao")]
    public int? acao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLog:idArea")]
    public int? idArea { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLog:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLog:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLog:idResponsavel")]
    public int? idResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLog:dataAgendamento")]
    public DateTime? dataAgendamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliLog:inssIdTipoBeneficio")]
    public int? inssIdTipoBeneficio { get; set; }
}
