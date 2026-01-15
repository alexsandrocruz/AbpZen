using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.CustomField.Dtos;

[Serializable]
public class CustomFieldDto : FullAuditedEntityDto<Guid>
{
    public string EntityType { get; set; }
    public string Label { get; set; }
    public string FieldType { get; set; }
    public string FieldKey { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
