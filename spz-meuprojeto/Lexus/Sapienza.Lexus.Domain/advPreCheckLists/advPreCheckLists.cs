// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPreCheckLists;

/// <summary>
/// advPreCheckLists entity
/// </summary>
public class advPreCheckLists : FullAuditedAggregateRoot<Guid>
{
    public int? idCheckList { get; set; }
    public int idGrupo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos> advPreCheckListses { get; set; } = new List<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos>();

    protected advPreCheckLists()
    {
        // Required by EF Core
    }

    public advPreCheckLists(Guid id) : base(id)
    {
    }
}
