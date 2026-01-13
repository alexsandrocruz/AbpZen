// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finCentrosResultado;

/// <summary>
/// finCentrosResultado entity
/// </summary>
public class finCentrosResultado : FullAuditedAggregateRoot<Guid>
{
    public int? idCentroResultado { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? padrao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finCentrosResultado()
    {
        // Required by EF Core
    }

    public finCentrosResultado(Guid id) : base(id)
    {
    }
}
