// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.Lawyer;

/// <summary>
/// Lawyer entity
/// </summary>
public class Lawyer : FullAuditedAggregateRoot<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string? PreferredName { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.LegalProcess.LegalProcess> Processes { get; set; } = new List<Sapienza.Lexus.LegalProcess.LegalProcess>();
    public virtual ICollection<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization> Specializations { get; set; } = new List<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization>();
    public virtual ICollection<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization> LawyerSpecializations { get; set; } = new List<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization>();

    protected Lawyer()
    {
        // Required by EF Core
    }

    public Lawyer(Guid id) : base(id)
    {
    }
}
