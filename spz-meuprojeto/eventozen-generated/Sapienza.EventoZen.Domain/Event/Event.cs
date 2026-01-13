// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.EventoZen.Event;

/// <summary>
/// Event entity
/// </summary>
public class Event : FullAuditedAggregateRoot<Guid>
{
    public Guid? LocalPartnerId { get; set; }
    public string? Title { get; set; }
    public EventType? Type { get; set; }
    public EventStatus? Status { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public decimal? Fee { get; set; }
    public string? Description { get; set; }
    public decimal? TaxPercentage { get; set; }
    public decimal? TaxValue { get; set; }
    public string? ContractType { get; set; }
    public string? NegotiationType { get; set; }
    public bool? HasConflict { get; set; }
    public Guid? SuggestedAlternativeArtistId { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ArtistId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? LocationId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.EventoZen.Artist.Artist? Artist { get; set; }
    public virtual Sapienza.EventoZen.Client.Client? Client { get; set; }
    public virtual Sapienza.EventoZen.Location.Location? Location { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.EventoZen.EventCommission.EventCommission> EventCommissions { get; set; } = new List<Sapienza.EventoZen.EventCommission.EventCommission>();

    protected Event()
    {
        // Required by EF Core
    }

    public Event(Guid id) : base(id)
    {
    }
}
