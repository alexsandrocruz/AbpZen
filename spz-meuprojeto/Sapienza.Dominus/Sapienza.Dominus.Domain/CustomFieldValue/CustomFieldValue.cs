// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.CustomFieldValue;

/// <summary>
/// CustomFieldValue entity
/// </summary>
public class CustomFieldValue : Entity<Guid>
{
    public string EntityId { get; set; } = string.Empty;
    public string ValueText { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? CustomFieldId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.CustomField.CustomField? CustomField { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected CustomFieldValue()
    {
        // Required by EF Core
    }

    public CustomFieldValue(Guid id) : base(id)
    {
    }
}
