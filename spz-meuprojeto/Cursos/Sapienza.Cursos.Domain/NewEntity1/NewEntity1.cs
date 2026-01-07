using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Cursos.NewEntity1;

/// <summary>
/// NewEntity1 entity
/// </summary>
public class NewEntity1 : FullAuditedAggregateRoot<Guid>
{

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected NewEntity1()
    {
        // Required by EF Core
    }

    public NewEntity1(Guid id) : base(id)
    {
    }
}
