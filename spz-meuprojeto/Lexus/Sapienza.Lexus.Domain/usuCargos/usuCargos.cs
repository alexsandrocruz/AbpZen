// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.usuCargos;

/// <summary>
/// usuCargos entity
/// </summary>
public class usuCargos : FullAuditedAggregateRoot<Guid>
{
    public int? idCargo { get; set; }
    public int idArea { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.usuAreas.usuAreas> usuCargoses { get; set; } = new List<Sapienza.Lexus.usuAreas.usuAreas>();

    protected usuCargos()
    {
        // Required by EF Core
    }

    public usuCargos(Guid id) : base(id)
    {
    }
}
