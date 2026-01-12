using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Cursos.Lawyer;

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
    public virtual ICollection<Sapienza.Cursos.LegalProcess.LegalProcess> Processes { get; set; } = new List<Sapienza.Cursos.LegalProcess.LegalProcess>();
    public virtual ICollection<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization> LawyerSpecializations { get; set; } = new List<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization>();

    protected Lawyer()
    {
        // Required by EF Core
    }

    public Lawyer(Guid id) : base(id)
    {
    }
}
