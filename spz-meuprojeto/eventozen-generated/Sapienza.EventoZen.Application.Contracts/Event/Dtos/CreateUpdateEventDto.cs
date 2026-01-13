using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.EventoZen.Event.Dtos;

[Serializable]
public class CreateUpdateEventDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public Guid? LocalPartnerId { get; set; }
    [StringLength(256)]
    public string Title { get; set; }
    public EventType? Type { get; set; }
    public EventStatus? Status { get; set; }
    [Required]
    public DateTime StartDateTime { get; set; }
    [Required]
    public DateTime EndDateTime { get; set; }
    public decimal? Fee { get; set; }
    [StringLength(2048)]
    public string Description { get; set; }
    public decimal? TaxPercentage { get; set; }
    public decimal? TaxValue { get; set; }
    [StringLength(50)]
    public string ContractType { get; set; }
    [StringLength(50)]
    public string NegotiationType { get; set; }
    public bool? HasConflict { get; set; }
    public Guid? SuggestedAlternativeArtistId { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ArtistId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? LocationId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
