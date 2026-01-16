// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.usuAcessos;

/// <summary>
/// usuAcessos entity
/// </summary>
public class usuAcessos : FullAuditedAggregateRoot<Guid>
{
    public int? idAcesso { get; set; }
    public Guid IdentityUserId { get; set; }
    public DateTime? data { get; set; }
    public string? ip { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected usuAcessos()
    {
        // Required by EF Core
    }

    public usuAcessos(Guid id) : base(id)
    {
    }
}
