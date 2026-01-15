// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.ProposalBlockInstance;

/// <summary>
/// ProposalBlockInstance entity
/// </summary>
public class ProposalBlockInstance : Entity<Guid>
{
    public string BlockType { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ProposalId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Proposal.Proposal? Proposal { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected ProposalBlockInstance()
    {
        // Required by EF Core
    }

    public ProposalBlockInstance(Guid id) : base(id)
    {
    }
}
