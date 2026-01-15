using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.BlogPostVersion.Dtos;

[Serializable]
public class BlogPostVersionDto : FullAuditedEntityDto<Guid>
{
    public int VersionNumber { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? BlogPostId { get; set; }
    public string? BlogPostDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
