// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advTarefas;

/// <summary>
/// advTarefas entity
/// </summary>
public class advTarefas : FullAuditedAggregateRoot<Guid>
{
    public int? idTarefa { get; set; }
    public int idTipoTarefa { get; set; }
    public int? idCompromisso { get; set; }
    public int? idProcesso { get; set; }
    public string? dataCadastro { get; set; }
    public string? dataParaFinalizacao { get; set; }
    public string? descricao { get; set; }
    public Guid? ResponsibleUserId { get; set; }
    public int? idExecutor { get; set; }
    public bool? finalizado { get; set; }
    public DateTime? tsFinalizacao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? incluidoPor { get; set; }
    public string? alteradoPor { get; set; }
    public bool? agendada { get; set; }
    public int? horarioInicial { get; set; }
    public int? horarioFinal { get; set; }
    public string? onde { get; set; }
    public int? idCliente { get; set; }
    public int? idUsuarioFinalizou { get; set; }
    public int? lembreteQuandoFinalizarPara { get; set; }
    public bool? tecnica { get; set; }
    public bool? coletivoOriginal { get; set; }
    public int? coletivoIdOriginal { get; set; }
    public int? coletivoIdCliente { get; set; }
    public bool? pauta { get; set; }
    public int? pautaIdUsuarioResp { get; set; }
    public bool? pautaRespAceite { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas> advTarefases { get; set; } = new List<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas>();
    public virtual ICollection<Sapienza.Lexus.advCompromissos.advCompromissos> advTarefasesCollection { get; set; } = new List<Sapienza.Lexus.advCompromissos.advCompromissos>();
    public virtual ICollection<Sapienza.Lexus.advProcessos.advProcessos> advTarefasesCollection1 { get; set; } = new List<Sapienza.Lexus.advProcessos.advProcessos>();

    protected advTarefas()
    {
        // Required by EF Core
    }

    public advTarefas(Guid id) : base(id)
    {
    }
}
