using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.flwConfig.Dtos;

[Serializable]
public class flwConfigDto : FullAuditedEntityDto<Guid>
{
    public int? idConfig { get; set; }
    public string tipoMarcacoes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
