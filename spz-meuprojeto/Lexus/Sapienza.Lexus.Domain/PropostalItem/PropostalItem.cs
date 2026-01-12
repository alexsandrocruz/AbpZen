#nullable enable
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.PropostalItem;

/// <summary>
/// PropostalItem entity
/// </summary>
public class PropostalItem : FullAuditedAggregateRoot<Guid>
{
    public string? Desc { get; set; }
    public decimal? Quant { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Total { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ProposalId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.Proposal.Proposal? Proposal { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected PropostalItem()
    {
        // Required by EF Core
    }

    public PropostalItem(Guid id) : base(id)
    {
    }
}
