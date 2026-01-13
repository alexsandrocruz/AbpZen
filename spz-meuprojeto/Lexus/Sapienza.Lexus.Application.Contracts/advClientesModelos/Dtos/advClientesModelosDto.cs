using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesModelos.Dtos;

[Serializable]
public class advClientesModelosDto : FullAuditedEntityDto<Guid>
{
    public int? idModelo { get; set; }
    public string titulo { get; set; }
    public string conteudo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
