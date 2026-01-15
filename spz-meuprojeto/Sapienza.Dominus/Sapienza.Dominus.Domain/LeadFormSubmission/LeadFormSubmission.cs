// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadFormSubmission;

/// <summary>
/// LeadFormSubmission entity
/// </summary>
public class LeadFormSubmission : Entity<Guid>
{
    public string IpAddress { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadFormId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.LeadForm.LeadForm? LeadForm { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LeadFormSubmission()
    {
        // Required by EF Core
    }

    public LeadFormSubmission(Guid id) : base(id)
    {
    }
}
