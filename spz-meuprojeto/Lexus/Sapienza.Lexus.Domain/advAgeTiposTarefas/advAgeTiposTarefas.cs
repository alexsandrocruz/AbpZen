// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advAgeTiposTarefas;

/// <summary>
/// advAgeTiposTarefas entity
/// </summary>
public class advAgeTiposTarefas : FullAuditedAggregateRoot<Guid>
{
    public int? idTipoTarefa { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? agendada { get; set; }
    public bool? pauta { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advAgeTiposTarefas()
    {
        // Required by EF Core
    }

    public advAgeTiposTarefas(Guid id) : base(id)
    {
    }
}
