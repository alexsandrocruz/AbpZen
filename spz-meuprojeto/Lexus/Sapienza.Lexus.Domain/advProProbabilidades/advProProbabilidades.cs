// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProProbabilidades;

/// <summary>
/// advProProbabilidades entity
/// </summary>
public class advProProbabilidades : FullAuditedAggregateRoot<Guid>
{
    public int? idProbabilidade { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProProbabilidades()
    {
        // Required by EF Core
    }

    public advProProbabilidades(Guid id) : base(id)
    {
    }
}
