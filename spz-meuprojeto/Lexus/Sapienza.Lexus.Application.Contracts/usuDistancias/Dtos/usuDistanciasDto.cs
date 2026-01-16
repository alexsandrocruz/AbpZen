using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.usuDistancias.Dtos;

[Serializable]
public class usuDistanciasDto : FullAuditedEntityDto<Guid>
{
    public int? idDistancia { get; set; }
    public Guid IdentityUserId { get; set; }
    public string estado { get; set; }
    public string cidade { get; set; }
    public int? km { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
