using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.Case.Dtos;

[Serializable]
public class CaseDto : FullAuditedEntityDto<Guid>
{
    public string CaseNumber { get; set; }
    public string Title { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
