using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabCidades.Dtos;

[Serializable]
public class fabCidadesDto : FullAuditedEntityDto<Guid>
{
    public int? idCidade { get; set; }
    public string descricao { get; set; }
    public string codigoIBGE { get; set; }
    public int? idEstado { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
