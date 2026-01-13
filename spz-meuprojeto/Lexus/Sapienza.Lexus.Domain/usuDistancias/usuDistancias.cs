// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.usuDistancias;

/// <summary>
/// usuDistancias entity
/// </summary>
public class usuDistancias : FullAuditedAggregateRoot<Guid>
{
    public int? idDistancia { get; set; }
    public int idUsuario { get; set; }
    public string? estado { get; set; }
    public string? cidade { get; set; }
    public int? km { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected usuDistancias()
    {
        // Required by EF Core
    }

    public usuDistancias(Guid id) : base(id)
    {
    }
}
