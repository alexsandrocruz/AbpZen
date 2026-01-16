// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advClientesINSSStatus;

/// <summary>
/// advClientesINSSStatus entity
/// </summary>
public class advClientesINSSStatus : FullAuditedAggregateRoot<Guid>
{
    public int? idStatus { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid advClientesINSSId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.advClientesINSS.advClientesINSS advClientesINSSStatusNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advClientesINSSStatus()
    {
        // Required by EF Core
    }

    public advClientesINSSStatus(Guid id) : base(id)
    {
    }
}
