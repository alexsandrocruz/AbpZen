#nullable enable
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.Proposal;

/// <summary>
/// Proposal entity
/// </summary>
public class Proposal : FullAuditedAggregateRoot<Guid>
{
    public string? Number { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? ValidityDate { get; set; }
    public string? Obs { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ClientId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.Client.Client? Client { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.PropostalItem.PropostalItem> PropostalItems { get; set; } = new List<Sapienza.Lexus.PropostalItem.PropostalItem>();

    protected Proposal()
    {
        // Required by EF Core
    }

    public Proposal(Guid id) : base(id)
    {
    }
}
