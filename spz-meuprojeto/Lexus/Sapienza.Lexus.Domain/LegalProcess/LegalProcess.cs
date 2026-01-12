using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.LegalProcess;

/// <summary>
/// LegalProcess entity
/// </summary>
public class LegalProcess : FullAuditedAggregateRoot<Guid>
{
    public string ProcessNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DateOpened { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid LawyerId { get; set; }
    public Guid ClientId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.Lawyer.Lawyer Lawyer { get; set; }
    public virtual Sapienza.Lexus.Client.Client Client { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LegalProcess()
    {
        // Required by EF Core
    }

    public LegalProcess(Guid id) : base(id)
    {
    }
}
