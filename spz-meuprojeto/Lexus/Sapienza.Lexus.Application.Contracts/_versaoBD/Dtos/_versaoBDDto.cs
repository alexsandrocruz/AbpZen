using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus._versaoBD.Dtos;

[Serializable]
public class _versaoBDDto : FullAuditedEntityDto<Guid>
{
    public int? id { get; set; }
    public string arquivo { get; set; }
    public DateTime? dataAplicacao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
