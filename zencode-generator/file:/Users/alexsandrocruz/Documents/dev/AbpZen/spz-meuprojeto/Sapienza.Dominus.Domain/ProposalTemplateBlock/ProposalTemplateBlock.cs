// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.ProposalTemplateBlock;

/// <summary>
/// ProposalTemplateBlock entity
/// </summary>
public class ProposalTemplateBlock : Entity<Guid>
{
    public string BlockType { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected ProposalTemplateBlock()
    {
        // Required by EF Core
    }

    public ProposalTemplateBlock(Guid id) : base(id)
    {
    }
}
