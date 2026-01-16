// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabMotivosAproveitamento;

/// <summary>
/// fabMotivosAproveitamento entity
/// </summary>
public class fabMotivosAproveitamento : FullAuditedAggregateRoot<Guid>
{
    public int? idMotivo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabMotivosAproveitamento()
    {
        // Required by EF Core
    }

    public fabMotivosAproveitamento(Guid id) : base(id)
    {
    }
}
