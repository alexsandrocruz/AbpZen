using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCliBairros.Dtos;

[Serializable]
public class advCliBairrosDto : FullAuditedEntityDto<Guid>
{
    public int? idBairro { get; set; }
    public string titulo { get; set; }
    public string cidade { get; set; }
    public string estado { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
