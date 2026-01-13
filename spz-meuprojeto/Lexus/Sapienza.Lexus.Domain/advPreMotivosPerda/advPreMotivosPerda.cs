// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPreMotivosPerda;

/// <summary>
/// advPreMotivosPerda entity
/// </summary>
public class advPreMotivosPerda : FullAuditedAggregateRoot<Guid>
{
    public int? idMotivo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advPreMotivosPerda()
    {
        // Required by EF Core
    }

    public advPreMotivosPerda(Guid id) : base(id)
    {
    }
}
