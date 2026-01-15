// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadFormField;

/// <summary>
/// LeadFormField entity
/// </summary>
public class LeadFormField : Entity<Guid>
{
    public string Label { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadFormId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.LeadForm.LeadForm? LeadForm { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LeadFormField()
    {
        // Required by EF Core
    }

    public LeadFormField(Guid id) : base(id)
    {
    }
}
