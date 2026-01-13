using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.Artist.Dtos;

[Serializable]
public class ArtistGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Name { get; set; }
    public ArtistType? Type { get; set; }
    public string? Biography { get; set; }
    public string? PhotoUrl { get; set; }
    public bool? IsActive { get; set; }
    public string? InstagramHandle { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? HexColor { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
