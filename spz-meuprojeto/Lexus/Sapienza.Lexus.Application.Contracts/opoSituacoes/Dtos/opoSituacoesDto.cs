using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.opoSituacoes.Dtos;

[Serializable]
public class opoSituacoesDto : FullAuditedEntityDto<Guid>
{
    public int? idSituacao { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? ordem { get; set; }
    public bool? considerarIndicador { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? opoOportunidadesId { get; set; }
    public string? opoOportunidadesDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
