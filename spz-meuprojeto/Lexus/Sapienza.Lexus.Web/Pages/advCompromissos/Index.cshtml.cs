using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCompromissos;
using Sapienza.Lexus.advCompromissos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCompromissos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCompromissosFilterInput advCompromissosFilter { get; set; }
    
    private readonly IadvCompromissosAppService _advCompromissosAppService;

    public IndexModel(IadvCompromissosAppService advCompromissosAppService)
    {
        _advCompromissosAppService = advCompromissosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCompromissosGetListInput input)
    {
        var result = await _advCompromissosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCompromissosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCompromissosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:idCompromisso")]
    public int? idCompromisso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:idTipoCompromisso")]
    public int? idTipoCompromisso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:dataPublicacao")]
    public string? dataPublicacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:dataPrazoInterno")]
    public string? dataPrazoInterno { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:dataPrazoFatal")]
    public string? dataPrazoFatal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:descricao")]
    public string? descricao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:incluidoPor")]
    public string? incluidoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:alteradoPor")]
    public string? alteradoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:idAgendamentoINSS")]
    public int? idAgendamentoINSS { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:pauta")]
    public bool? pauta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:pautaIdUsuarioResp")]
    public int? pautaIdUsuarioResp { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:pautaRespAceite")]
    public bool? pautaRespAceite { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:horarioInicial")]
    public int? horarioInicial { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCompromissos:horarioFinal")]
    public int? horarioFinal { get; set; }
}
