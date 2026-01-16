using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabRegioes.Dtos;

[Serializable]
public class fabRegioesDto : FullAuditedEntityDto<Guid>
{
    public int? idRegiao { get; set; }
    public string titulo { get; set; }
    public string estados { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
