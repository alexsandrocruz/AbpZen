using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advAgeTiposTarefas.Dtos;

[Serializable]
public class advAgeTiposTarefasDto : FullAuditedEntityDto<Guid>
{
    public int? idTipoTarefa { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? agendada { get; set; }
    public bool? pauta { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
