using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesINSSStatus.Dtos;

[Serializable]
public class advClientesINSSStatusDto : FullAuditedEntityDto<Guid>
{
    public int? idStatus { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid advClientesINSSId { get; set; }
    public string? advClientesINSSDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
