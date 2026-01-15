// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Contract;

/// <summary>
/// Contract entity
/// </summary>
public class Contract : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; }
    public string Status { get; set; }
    public double TotalValue { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ClientId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Client.Client? Client { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Contract()
    {
        // Required by EF Core
    }

    public Contract(Guid id) : base(id)
    {
    }
}
