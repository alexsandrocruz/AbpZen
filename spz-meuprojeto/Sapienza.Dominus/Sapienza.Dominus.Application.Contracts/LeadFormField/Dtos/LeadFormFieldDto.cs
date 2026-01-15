using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadFormField.Dtos;

[Serializable]
public class LeadFormFieldDto : FullAuditedEntityDto<Guid>
{
    public string Label { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadFormId { get; set; }
    public string? LeadFormDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
