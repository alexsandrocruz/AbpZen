// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advCompromissos;

/// <summary>
/// advCompromissos entity
/// </summary>
public class advCompromissos : FullAuditedAggregateRoot<Guid>
{
    public int? idCompromisso { get; set; }
    public int idTipoCompromisso { get; set; }
    public int? idProcesso { get; set; }
    public string? dataPublicacao { get; set; }
    public string? dataPrazoInterno { get; set; }
    public string? dataPrazoFatal { get; set; }
    public string? descricao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? incluidoPor { get; set; }
    public string? alteradoPor { get; set; }
    public int? idAgendamentoINSS { get; set; }
    public bool? pauta { get; set; }
    public int? pautaIdUsuarioResp { get; set; }
    public bool? pautaRespAceite { get; set; }
    public int? horarioInicial { get; set; }
    public int? horarioFinal { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid advTarefasId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.advTarefas.advTarefas advCompromissosNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos> advCompromissoses { get; set; } = new List<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos>();
    public virtual ICollection<Sapienza.Lexus.advProcessos.advProcessos> advCompromissosesCollection { get; set; } = new List<Sapienza.Lexus.advProcessos.advProcessos>();

    protected advCompromissos()
    {
        // Required by EF Core
    }

    public advCompromissos(Guid id) : base(id)
    {
    }
}
