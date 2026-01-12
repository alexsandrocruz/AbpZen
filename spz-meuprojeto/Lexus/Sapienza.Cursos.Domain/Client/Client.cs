using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Cursos.Client;

/// <summary>
/// Client entity
/// </summary>
public class Client : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string CpfCnpj { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Cursos.LegalProcess.LegalProcess> Processes { get; set; } = new List<Sapienza.Cursos.LegalProcess.LegalProcess>();
    public virtual ICollection<Sapienza.Cursos.Proposal.Proposal> Proposals { get; set; } = new List<Sapienza.Cursos.Proposal.Proposal>();

    protected Client()
    {
        // Required by EF Core
    }

    public Client(Guid id) : base(id)
    {
    }
}
