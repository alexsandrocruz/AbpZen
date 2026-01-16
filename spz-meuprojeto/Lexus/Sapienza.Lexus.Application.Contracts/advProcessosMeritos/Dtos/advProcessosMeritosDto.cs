using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProcessosMeritos.Dtos;

[Serializable]
public class advProcessosMeritosDto : FullAuditedEntityDto<Guid>
{
    public int? idProcessoMerito { get; set; }
    public int idProcesso { get; set; }
    public int idMerito { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
