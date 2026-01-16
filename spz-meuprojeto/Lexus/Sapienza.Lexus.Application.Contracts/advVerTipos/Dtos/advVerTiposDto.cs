using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advVerTipos.Dtos;

[Serializable]
public class advVerTiposDto : FullAuditedEntityDto<Guid>
{
    public int? idTipo { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? advVerbasId { get; set; }
    public string? advVerbasDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
