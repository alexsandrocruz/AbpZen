using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPautaObs.Dtos;

[Serializable]
public class advPautaObsDto : FullAuditedEntityDto<Guid>
{
    public int? idPautaObs { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? id { get; set; }
    public string idTipo { get; set; }
    public string observacao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
