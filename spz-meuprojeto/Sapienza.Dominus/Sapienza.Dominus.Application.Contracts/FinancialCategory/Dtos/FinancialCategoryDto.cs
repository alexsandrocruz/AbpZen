using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.FinancialCategory.Dtos;

[Serializable]
public class FinancialCategoryDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Color { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
