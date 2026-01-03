using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.Lead;

/// <summary>
/// Lead entity
/// </summary>
public class Lead : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<LeptonXDemoApp.LeadContact.LeadContact> LeadContacts { get; set; } = new List<LeptonXDemoApp.LeadContact.LeadContact>();

    protected Lead()
    {
        // Required by EF Core
    }

    public Lead(Guid id) : base(id)
    {
    }
}
