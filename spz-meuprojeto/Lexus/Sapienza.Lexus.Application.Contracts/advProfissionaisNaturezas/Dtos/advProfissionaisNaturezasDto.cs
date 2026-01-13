using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProfissionaisNaturezas.Dtos;

[Serializable]
public class advProfissionaisNaturezasDto : FullAuditedEntityDto<Guid>
{
    public int? idProfissionalNatureza { get; set; }
    public int idProfissional { get; set; }
    public int idNatureza { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
