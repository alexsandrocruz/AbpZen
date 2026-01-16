// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finAreas;

/// <summary>
/// finAreas entity
/// </summary>
public class finAreas : FullAuditedAggregateRoot<Guid>
{
    public int? idArea { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idCentroResultado { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? finLancamentosId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.finLancamentos.finLancamentos? finAreasNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finAreas()
    {
        // Required by EF Core
    }

    public finAreas(Guid id) : base(id)
    {
    }
}
