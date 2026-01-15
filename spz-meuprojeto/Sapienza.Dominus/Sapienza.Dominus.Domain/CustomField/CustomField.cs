// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.CustomField;

/// <summary>
/// CustomField entity
/// </summary>
public class CustomField : FullAuditedAggregateRoot<Guid>
{
    public string EntityType { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public string FieldKey { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.CustomFieldValue.CustomFieldValue> CustomFieldValues { get; set; } = new List<Dominus.CustomFieldValue.CustomFieldValue>();

    protected CustomField()
    {
        // Required by EF Core
    }

    public CustomField(Guid id) : base(id)
    {
    }
}
