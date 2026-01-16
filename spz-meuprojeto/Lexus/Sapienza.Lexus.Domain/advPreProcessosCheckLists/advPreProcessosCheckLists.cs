// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPreProcessosCheckLists;

/// <summary>
/// advPreProcessosCheckLists entity
/// </summary>
public class advPreProcessosCheckLists : FullAuditedAggregateRoot<Guid>
{
    public int? idPreCheckList { get; set; }
    public int? idProcesso { get; set; }
    public int? idGrupo { get; set; }
    public int? idCheckList { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string? grupo { get; set; }
    public string? item { get; set; }
    public bool? concluido { get; set; }
    public string? tsConclusao { get; set; }
    public Guid? ResponsibleUserId { get; set; }
    public int? ordem { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advPreProcessosCheckLists()
    {
        // Required by EF Core
    }

    public advPreProcessosCheckLists(Guid id) : base(id)
    {
    }
}
