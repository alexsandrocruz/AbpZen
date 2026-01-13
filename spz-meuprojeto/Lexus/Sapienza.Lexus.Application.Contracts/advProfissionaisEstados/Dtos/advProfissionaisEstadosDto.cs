using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProfissionaisEstados.Dtos;

[Serializable]
public class advProfissionaisEstadosDto : FullAuditedEntityDto<Guid>
{
    public int? idProfissionalEstado { get; set; }
    public int idProfissional { get; set; }
    public string estado { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
