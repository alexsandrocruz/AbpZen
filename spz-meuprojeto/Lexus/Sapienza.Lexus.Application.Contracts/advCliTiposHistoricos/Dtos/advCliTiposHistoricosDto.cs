using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCliTiposHistoricos.Dtos;

[Serializable]
public class advCliTiposHistoricosDto : FullAuditedEntityDto<Guid>
{
    public int? idTipoHistorico { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? advClientesHistoricosId { get; set; }
    public string? advClientesHistoricosDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
