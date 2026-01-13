using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.EventoZen.Artist.Dtos;

[Serializable]
public class CreateUpdateArtistDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public ArtistType Type { get; set; }
    [StringLength(2048)]
    public string Biography { get; set; }
    [StringLength(256)]
    public string PhotoUrl { get; set; }
    public bool? IsActive { get; set; }
    [StringLength(256)]
    public string InstagramHandle { get; set; }
    [StringLength(256)]
    public string WebsiteUrl { get; set; }
    [StringLength(256)]
    public string LogoUrl { get; set; }
    [StringLength(256)]
    public string BannerUrl { get; set; }
    [StringLength(7)]
    public string HexColor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
