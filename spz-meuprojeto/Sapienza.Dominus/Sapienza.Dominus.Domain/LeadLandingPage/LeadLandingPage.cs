// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadLandingPage;

/// <summary>
/// LeadLandingPage entity
/// </summary>
public class LeadLandingPage : FullAuditedEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadWorkflowId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.LeadWorkflow.LeadWorkflow? LeadWorkflow { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LeadLandingPage()
    {
        // Required by EF Core
    }

    public LeadLandingPage(Guid id) : base(id)
    {
    }
}
