using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabHistoricoTipos.Dtos;

[Serializable]
public class fabHistoricoTiposDto : FullAuditedEntityDto<Guid>
{
    public int? idHistoricoTipo { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string tipoMarcacoes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? flwConfigExcecoesId { get; set; }
    public string? flwConfigExcecoesDisplayName { get; set; }
    public Guid? flwGradeHorariosId { get; set; }
    public string? flwGradeHorariosDisplayName { get; set; }
    public Guid? flwFollowsId { get; set; }
    public string? flwFollowsDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
