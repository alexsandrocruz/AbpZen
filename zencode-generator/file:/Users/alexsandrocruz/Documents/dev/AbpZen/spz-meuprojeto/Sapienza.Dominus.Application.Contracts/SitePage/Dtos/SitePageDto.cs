using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SitePage.Dtos;

[Serializable]
public class SitePageDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Content { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SiteId { get; set; }
    public string? SiteDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
