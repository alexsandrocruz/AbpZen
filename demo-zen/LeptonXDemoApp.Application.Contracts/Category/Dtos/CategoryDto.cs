using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Category.Dtos;

[Serializable]
public class CategoryDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
}
