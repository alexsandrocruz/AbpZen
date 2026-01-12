using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.LeadContact;

/// <summary>
/// LeadContact entity
/// </summary>
public class LeadContact : FullAuditedAggregateRoot<Guid>
{
    public string? Name { get; set; }
    public string? Position { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadId { get; set; }

    // ========== Navigation Properties ==========
    public virtual LeptonXDemoApp.Lead.Lead? Lead { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LeadContact()
    {
        // Required by EF Core
    }

    public LeadContact(Guid id) : base(id)
    {
    }
}
