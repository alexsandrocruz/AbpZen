using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Site.Dtos;

[Serializable]
public class SiteDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
