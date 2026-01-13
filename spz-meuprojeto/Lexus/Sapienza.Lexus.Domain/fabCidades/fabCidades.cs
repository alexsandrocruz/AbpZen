// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabCidades;

/// <summary>
/// fabCidades entity
/// </summary>
public class fabCidades : FullAuditedAggregateRoot<Guid>
{
    public int? idCidade { get; set; }
    public string? descricao { get; set; }
    public string? codigoIBGE { get; set; }
    public int? idEstado { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabCidades()
    {
        // Required by EF Core
    }

    public fabCidades(Guid id) : base(id)
    {
    }
}
