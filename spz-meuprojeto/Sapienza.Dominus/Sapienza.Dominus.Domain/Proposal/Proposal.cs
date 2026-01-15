// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Proposal;

/// <summary>
/// Proposal entity
/// </summary>
public class Proposal : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; }
    public string Status { get; set; }
    public DateTime ValidUntil { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ClientId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Client.Client? Client { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.ProposalItem.ProposalItem> ProposalItems { get; set; } = new List<Dominus.ProposalItem.ProposalItem>();
    public virtual ICollection<Dominus.ProposalBlockInstance.ProposalBlockInstance> ProposalBlockInstances { get; set; } = new List<Dominus.ProposalBlockInstance.ProposalBlockInstance>();

    protected Proposal()
    {
        // Required by EF Core
    }

    public Proposal(Guid id) : base(id)
    {
    }
}
