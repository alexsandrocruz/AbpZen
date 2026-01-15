// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadForm;

/// <summary>
/// LeadForm entity
/// </summary>
public class LeadForm : FullAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadWorkflowId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.LeadWorkflow.LeadWorkflow? LeadWorkflow { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.LeadFormField.LeadFormField> LeadFormFields { get; set; } = new List<Dominus.LeadFormField.LeadFormField>();
    public virtual ICollection<Dominus.LeadFormSubmission.LeadFormSubmission> LeadFormSubmissions { get; set; } = new List<Dominus.LeadFormSubmission.LeadFormSubmission>();

    protected LeadForm()
    {
        // Required by EF Core
    }

    public LeadForm(Guid id) : base(id)
    {
    }
}
