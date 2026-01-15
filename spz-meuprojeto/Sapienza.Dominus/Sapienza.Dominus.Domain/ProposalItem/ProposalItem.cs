// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.ProposalItem;

/// <summary>
/// ProposalItem entity
/// </summary>
public class ProposalItem : Entity<Guid>
{
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double Price { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ProposalId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Proposal.Proposal? Proposal { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected ProposalItem()
    {
        // Required by EF Core
    }

    public ProposalItem(Guid id) : base(id)
    {
    }
}
