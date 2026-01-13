using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.Event.Dtos;

[Serializable]
public class EventDto : FullAuditedEntityDto<Guid>
{
    public Guid? LocalPartnerId { get; set; }
    public string Title { get; set; }
    public EventType? Type { get; set; }
    public EventStatus? Status { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public decimal? Fee { get; set; }
    public string Description { get; set; }
    public decimal? TaxPercentage { get; set; }
    public decimal? TaxValue { get; set; }
    public string ContractType { get; set; }
    public string NegotiationType { get; set; }
    public bool? HasConflict { get; set; }
    public Guid? SuggestedAlternativeArtistId { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ArtistId { get; set; }
    public string? ArtistDisplayName { get; set; }
    public Guid? ClientId { get; set; }
    public string? ClientDisplayName { get; set; }
    public Guid? LocationId { get; set; }
    public string? LocationDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
