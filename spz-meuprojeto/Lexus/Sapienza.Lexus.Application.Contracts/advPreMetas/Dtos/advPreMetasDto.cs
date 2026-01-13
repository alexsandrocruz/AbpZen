using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPreMetas.Dtos;

[Serializable]
public class advPreMetasDto : FullAuditedEntityDto<Guid>
{
    public int? idMeta { get; set; }
    public string tipo { get; set; }
    public int? idResponsavel { get; set; }
    public int? idEscritorio { get; set; }
    public int? qtde { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
