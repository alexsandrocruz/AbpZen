using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.Artist.Dtos;

[Serializable]
public class ArtistDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public ArtistType Type { get; set; }
    public string Biography { get; set; }
    public string PhotoUrl { get; set; }
    public bool? IsActive { get; set; }
    public string InstagramHandle { get; set; }
    public string WebsiteUrl { get; set; }
    public string LogoUrl { get; set; }
    public string BannerUrl { get; set; }
    public string HexColor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
