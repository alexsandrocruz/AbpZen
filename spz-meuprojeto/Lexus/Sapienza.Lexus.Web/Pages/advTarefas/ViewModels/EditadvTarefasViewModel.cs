using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advTarefas.ViewModels;

public class EditadvTarefasViewModel
{
    [Display(Name = "advTarefas:idTarefa")]
    public int? idTarefa { get; set; }
    [Required]
    [Display(Name = "advTarefas:idTipoTarefa")]
    public int idTipoTarefa { get; set; }
    [Display(Name = "advTarefas:idCompromisso")]
    public int? idCompromisso { get; set; }
    [Display(Name = "advTarefas:idProcesso")]
    public int? idProcesso { get; set; }
    [Display(Name = "advTarefas:dataCadastro")]
    public string? dataCadastro { get; set; }
    [Display(Name = "advTarefas:dataParaFinalizacao")]
    public string? dataParaFinalizacao { get; set; }
    [Display(Name = "advTarefas:descricao")]
    public string? descricao { get; set; }
    [Display(Name = "advTarefas:idResponsavel")]
    public int? idResponsavel { get; set; }
    [Display(Name = "advTarefas:idExecutor")]
    public int? idExecutor { get; set; }
    [Display(Name = "advTarefas:finalizado")]
    public bool? finalizado { get; set; }
    [Display(Name = "advTarefas:tsFinalizacao")]
    public DateTime? tsFinalizacao { get; set; }
    [Display(Name = "advTarefas:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advTarefas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advTarefas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advTarefas:incluidoPor")]
    public string? incluidoPor { get; set; }
    [Display(Name = "advTarefas:alteradoPor")]
    public string? alteradoPor { get; set; }
    [Display(Name = "advTarefas:agendada")]
    public bool? agendada { get; set; }
    [Display(Name = "advTarefas:horarioInicial")]
    public int? horarioInicial { get; set; }
    [Display(Name = "advTarefas:horarioFinal")]
    public int? horarioFinal { get; set; }
    [Display(Name = "advTarefas:onde")]
    public string? onde { get; set; }
    [Display(Name = "advTarefas:idCliente")]
    public int? idCliente { get; set; }
    [Display(Name = "advTarefas:idUsuarioFinalizou")]
    public int? idUsuarioFinalizou { get; set; }
    [Display(Name = "advTarefas:lembreteQuandoFinalizarPara")]
    public int? lembreteQuandoFinalizarPara { get; set; }
    [Display(Name = "advTarefas:tecnica")]
    public bool? tecnica { get; set; }
    [Display(Name = "advTarefas:coletivoOriginal")]
    public bool? coletivoOriginal { get; set; }
    [Display(Name = "advTarefas:coletivoIdOriginal")]
    public int? coletivoIdOriginal { get; set; }
    [Display(Name = "advTarefas:coletivoIdCliente")]
    public int? coletivoIdCliente { get; set; }
    [Display(Name = "advTarefas:pauta")]
    public bool? pauta { get; set; }
    [Display(Name = "advTarefas:pautaIdUsuarioResp")]
    public int? pautaIdUsuarioResp { get; set; }
    [Display(Name = "advTarefas:pautaRespAceite")]
    public bool? pautaRespAceite { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
