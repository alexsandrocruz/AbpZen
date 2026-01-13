// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.EventoZen.Artist;

/// <summary>
/// Artist entity
/// </summary>
public class Artist : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public ArtistType Type { get; set; }
    public string? Biography { get; set; }
    public string? PhotoUrl { get; set; }
    public bool? IsActive { get; set; }
    public string? InstagramHandle { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? HexColor { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.EventoZen.Event.Event> Events { get; set; } = new List<Sapienza.EventoZen.Event.Event>();
    public virtual ICollection<Sapienza.EventoZen.Availability.Availability> Availabilities { get; set; } = new List<Sapienza.EventoZen.Availability.Availability>();
    public virtual ICollection<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty> ArtistSpecialties { get; set; } = new List<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty>();

    protected Artist()
    {
        // Required by EF Core
    }

    public Artist(Guid id) : base(id)
    {
    }
}
