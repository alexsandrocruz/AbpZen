// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advAgeTiposCompromissos;

/// <summary>
/// advAgeTiposCompromissos entity
/// </summary>
public class advAgeTiposCompromissos : FullAuditedAggregateRoot<Guid>
{
    public int? idTipoCompromisso { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? recebimentoProcesso { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? advCompromissosId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.advCompromissos.advCompromissos? advAgeTiposCompromissosNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advAgeTiposCompromissos()
    {
        // Required by EF Core
    }

    public advAgeTiposCompromissos(Guid id) : base(id)
    {
    }
}
