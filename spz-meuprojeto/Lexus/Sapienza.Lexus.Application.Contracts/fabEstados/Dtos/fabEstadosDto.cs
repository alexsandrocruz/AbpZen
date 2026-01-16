using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabEstados.Dtos;

[Serializable]
public class fabEstadosDto : FullAuditedEntityDto<Guid>
{
    public int? idEstado { get; set; }
    public string sigla { get; set; }
    public string descricao { get; set; }
    public int? idPais { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? fabCidadesId { get; set; }
    public string? fabCidadesDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
