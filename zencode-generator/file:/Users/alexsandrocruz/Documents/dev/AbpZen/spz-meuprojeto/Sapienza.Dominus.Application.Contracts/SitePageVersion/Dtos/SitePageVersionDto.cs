using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SitePageVersion.Dtos;

[Serializable]
public class SitePageVersionDto : FullAuditedEntityDto<Guid>
{
    public int VersionNumber { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SitePageId { get; set; }
    public string? SitePageDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
