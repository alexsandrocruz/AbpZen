// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPreLogStatus;

/// <summary>
/// advPreLogStatus entity
/// </summary>
public class advPreLogStatus : FullAuditedAggregateRoot<Guid>
{
    public int? idLog { get; set; }
    public int idProcesso { get; set; }
    public int idStatus { get; set; }
    public DateTime? tsInclusao { get; set; }
    public bool? conversao { get; set; }
    public DateTime? tsConversao { get; set; }
    public bool? perdido { get; set; }
    public DateTime? tsPerdido { get; set; }
    public int? diasCorridosDoAnterior { get; set; }
    public string? usuario { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advPreLogStatus()
    {
        // Required by EF Core
    }

    public advPreLogStatus(Guid id) : base(id)
    {
    }
}
