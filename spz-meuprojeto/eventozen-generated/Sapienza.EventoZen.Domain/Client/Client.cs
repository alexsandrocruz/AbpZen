// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.EventoZen.Client;

/// <summary>
/// Client entity
/// </summary>
public class Client : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public ClientType Type { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Notes { get; set; }
    public bool? IsActive { get; set; }
    public string? LeadStatus { get; set; }
    public DateTime? FirstContactDate { get; set; }
    public DateTime? LastContactDate { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.EventoZen.Event.Event> Events { get; set; } = new List<Sapienza.EventoZen.Event.Event>();

    protected Client()
    {
        // Required by EF Core
    }

    public Client(Guid id) : base(id)
    {
    }
}
