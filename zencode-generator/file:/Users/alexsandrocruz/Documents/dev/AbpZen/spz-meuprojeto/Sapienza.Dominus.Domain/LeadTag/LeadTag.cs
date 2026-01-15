// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadTag;

/// <summary>
/// LeadTag entity
/// </summary>
public class LeadTag : AuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LeadTag()
    {
        // Required by EF Core
    }

    public LeadTag(Guid id) : base(id)
    {
    }
}
