using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPostosINSS.Dtos;

[Serializable]
public class advPostosINSSDto : FullAuditedEntityDto<Guid>
{
    public int? idPosto { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid advClientesId { get; set; }
    public string? advClientesDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
