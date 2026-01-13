// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finPlanoContasDet;

/// <summary>
/// finPlanoContasDet entity
/// </summary>
public class finPlanoContasDet : FullAuditedAggregateRoot<Guid>
{
    public int? idPlanoContasDet { get; set; }
    public int idPlanoConta { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finPlanoContasDet()
    {
        // Required by EF Core
    }

    public finPlanoContasDet(Guid id) : base(id)
    {
    }
}
