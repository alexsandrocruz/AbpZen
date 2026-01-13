// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabHistoricoTipos;

/// <summary>
/// fabHistoricoTipos entity
/// </summary>
public class fabHistoricoTipos : FullAuditedAggregateRoot<Guid>
{
    public int? idHistoricoTipo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? tipoMarcacoes { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabHistoricoTipos()
    {
        // Required by EF Core
    }

    public fabHistoricoTipos(Guid id) : base(id)
    {
    }
}
