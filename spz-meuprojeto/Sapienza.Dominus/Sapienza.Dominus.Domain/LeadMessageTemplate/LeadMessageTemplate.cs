// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadMessageTemplate;

/// <summary>
/// LeadMessageTemplate entity
/// </summary>
public class LeadMessageTemplate : FullAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadWorkflowId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.LeadWorkflow.LeadWorkflow? LeadWorkflow { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LeadMessageTemplate()
    {
        // Required by EF Core
    }

    public LeadMessageTemplate(Guid id) : base(id)
    {
    }
}
