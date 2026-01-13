using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advTarefas;
using Sapienza.Lexus.advTarefas.Dtos;

namespace Sapienza.Lexus.Web.Pages.advTarefas;

public class IndexModel : Sapienza.LexusPageModel
{
    public advTarefasFilterInput advTarefasFilter { get; set; }
    
    private readonly IadvTarefasAppService _advTarefasAppService;

    public IndexModel(IadvTarefasAppService advTarefasAppService)
    {
        _advTarefasAppService = advTarefasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advTarefasGetListInput input)
    {
        var result = await _advTarefasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advTarefasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advTarefasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:idTarefa")]
    public int? idTarefa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:idTipoTarefa")]
    public int? idTipoTarefa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:idCompromisso")]
    public int? idCompromisso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:dataCadastro")]
    public string? dataCadastro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:dataParaFinalizacao")]
    public string? dataParaFinalizacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:descricao")]
    public string? descricao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:idResponsavel")]
    public int? idResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:idExecutor")]
    public int? idExecutor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:finalizado")]
    public bool? finalizado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:tsFinalizacao")]
    public DateTime? tsFinalizacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:incluidoPor")]
    public string? incluidoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:alteradoPor")]
    public string? alteradoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:agendada")]
    public bool? agendada { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:horarioInicial")]
    public int? horarioInicial { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:horarioFinal")]
    public int? horarioFinal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:onde")]
    public string? onde { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:idUsuarioFinalizou")]
    public int? idUsuarioFinalizou { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:lembreteQuandoFinalizarPara")]
    public int? lembreteQuandoFinalizarPara { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:tecnica")]
    public bool? tecnica { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:coletivoOriginal")]
    public bool? coletivoOriginal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:coletivoIdOriginal")]
    public int? coletivoIdOriginal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:coletivoIdCliente")]
    public int? coletivoIdCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:pauta")]
    public bool? pauta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:pautaIdUsuarioResp")]
    public int? pautaIdUsuarioResp { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefas:pautaRespAceite")]
    public bool? pautaRespAceite { get; set; }
}
