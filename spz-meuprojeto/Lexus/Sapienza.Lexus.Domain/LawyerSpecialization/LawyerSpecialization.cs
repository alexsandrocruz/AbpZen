using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.LawyerSpecialization;

/// <summary>
/// LawyerSpecialization entity
/// </summary>
public class LawyerSpecialization : FullAuditedAggregateRoot<Guid>
{

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid LawyerId { get; set; }
    public Guid SpecializationId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.Lawyer.Lawyer Lawyer { get; set; }
    public virtual Sapienza.Lexus.Specialization.Specialization Specialization { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LawyerSpecialization()
    {
        // Required by EF Core
    }

    public LawyerSpecialization(Guid id) : base(id)
    {
    }
}
