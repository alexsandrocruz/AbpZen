using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Cursos.Lawyer.Dtos;

[Serializable]
public class LawyerDto : FullAuditedEntityDto<Guid>
{
    public string FullName { get; set; }
    public string PreferredName { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
