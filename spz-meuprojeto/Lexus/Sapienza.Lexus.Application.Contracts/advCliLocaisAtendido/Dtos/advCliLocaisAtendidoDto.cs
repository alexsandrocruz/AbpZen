using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCliLocaisAtendido.Dtos;

[Serializable]
public class advCliLocaisAtendidoDto : FullAuditedEntityDto<Guid>
{
    public int? idLocalAtendido { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
