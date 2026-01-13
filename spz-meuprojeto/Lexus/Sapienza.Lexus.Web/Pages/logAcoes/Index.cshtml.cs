using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.logAcoes;
using Sapienza.Lexus.logAcoes.Dtos;

namespace Sapienza.Lexus.Web.Pages.logAcoes;

public class IndexModel : Sapienza.LexusPageModel
{
    public logAcoesFilterInput logAcoesFilter { get; set; }
    
    private readonly IlogAcoesAppService _logAcoesAppService;

    public IndexModel(IlogAcoesAppService logAcoesAppService)
    {
        _logAcoesAppService = logAcoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(logAcoesGetListInput input)
    {
        var result = await _logAcoesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _logAcoesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class logAcoesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logAcoes:idLog")]
    public int? idLog { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logAcoes:area")]
    public string? area { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logAcoes:acao")]
    public string? acao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logAcoes:usuario")]
    public string? usuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logAcoes:motivo")]
    public string? motivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logAcoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logAcoes:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logAcoes:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logAcoes:idCompromisso")]
    public int? idCompromisso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "logAcoes:idTarefa")]
    public int? idTarefa { get; set; }
}
