using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.opoTipos.Dtos;

[Serializable]
public class opoTiposDto : FullAuditedEntityDto<Guid>
{
    public int? idTipo { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? opoOportunidadesId { get; set; }
    public string? opoOportunidadesDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
