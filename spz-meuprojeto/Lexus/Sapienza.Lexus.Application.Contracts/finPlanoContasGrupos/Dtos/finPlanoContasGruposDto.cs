using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finPlanoContasGrupos.Dtos;

[Serializable]
public class finPlanoContasGruposDto : FullAuditedEntityDto<Guid>
{
    public int? idGrupo { get; set; }
    public string titulo { get; set; }
    public string tipo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idGrupoDRE { get; set; }
    public int? ordem { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
