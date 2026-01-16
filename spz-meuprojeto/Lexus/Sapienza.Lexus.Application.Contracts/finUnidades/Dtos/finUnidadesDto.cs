using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finUnidades.Dtos;

[Serializable]
public class finUnidadesDto : FullAuditedEntityDto<Guid>
{
    public int? idUnidade { get; set; }
    public string titulo { get; set; }
    public double? percentual { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
