// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.ClientContact;

/// <summary>
/// ClientContact entity
/// </summary>
public class ClientContact : FullAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ClientId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Client.Client? Client { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected ClientContact()
    {
        // Required by EF Core
    }

    public ClientContact(Guid id) : base(id)
    {
    }
}
