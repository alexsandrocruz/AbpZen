using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCliPrioridades.Dtos;

[Serializable]
public class advCliPrioridadesDto : FullAuditedEntityDto<Guid>
{
    public int? idPrioridade { get; set; }
    public string titulo { get; set; }
    public string cor { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
