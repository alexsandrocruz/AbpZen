// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProcessosMeritos;

/// <summary>
/// advProcessosMeritos entity
/// </summary>
public class advProcessosMeritos : FullAuditedAggregateRoot<Guid>
{
    public int? idProcessoMerito { get; set; }
    public int idProcesso { get; set; }
    public int idMerito { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advProMeritos.advProMeritos> advProcessosMeritoses { get; set; } = new List<Sapienza.Lexus.advProMeritos.advProMeritos>();
    public virtual ICollection<Sapienza.Lexus.advProcessos.advProcessos> advProcessosMeritosesCollection { get; set; } = new List<Sapienza.Lexus.advProcessos.advProcessos>();

    protected advProcessosMeritos()
    {
        // Required by EF Core
    }

    public advProcessosMeritos(Guid id) : base(id)
    {
    }
}
