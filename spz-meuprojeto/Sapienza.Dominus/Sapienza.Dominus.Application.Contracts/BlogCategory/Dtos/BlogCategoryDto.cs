using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.BlogCategory.Dtos;

[Serializable]
public class BlogCategoryDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
