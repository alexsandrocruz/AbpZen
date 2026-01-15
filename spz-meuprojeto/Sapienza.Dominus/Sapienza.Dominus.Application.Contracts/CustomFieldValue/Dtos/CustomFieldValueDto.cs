using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.CustomFieldValue.Dtos;

[Serializable]
public class CustomFieldValueDto : FullAuditedEntityDto<Guid>
{
    public string EntityId { get; set; }
    public string ValueText { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? CustomFieldId { get; set; }
    public string? CustomFieldDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
