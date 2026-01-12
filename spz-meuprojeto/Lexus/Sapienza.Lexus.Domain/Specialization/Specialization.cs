#nullable enable
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.Specialization;

/// <summary>
/// Specialization entity
/// </summary>
public class Specialization : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization> LawyerSpecializations { get; set; } = new List<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization>();

    protected Specialization()
    {
        // Required by EF Core
    }

    public Specialization(Guid id) : base(id)
    {
    }
}
