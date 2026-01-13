// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProfissionaisNaturezas;

/// <summary>
/// advProfissionaisNaturezas entity
/// </summary>
public class advProfissionaisNaturezas : FullAuditedAggregateRoot<Guid>
{
    public int? idProfissionalNatureza { get; set; }
    public int idProfissional { get; set; }
    public int idNatureza { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProfissionaisNaturezas()
    {
        // Required by EF Core
    }

    public advProfissionaisNaturezas(Guid id) : base(id)
    {
    }
}
