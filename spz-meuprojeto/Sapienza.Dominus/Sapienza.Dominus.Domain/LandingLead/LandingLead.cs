// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LandingLead;

/// <summary>
/// LandingLead entity
/// </summary>
public class LandingLead : FullAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LandingLead()
    {
        // Required by EF Core
    }

    public LandingLead(Guid id) : base(id)
    {
    }
}
