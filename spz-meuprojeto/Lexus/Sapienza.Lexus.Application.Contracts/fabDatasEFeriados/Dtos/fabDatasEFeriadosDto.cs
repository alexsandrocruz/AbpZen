using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabDatasEFeriados.Dtos;

[Serializable]
public class fabDatasEFeriadosDto : FullAuditedEntityDto<Guid>
{
    public int? idData { get; set; }
    public string titulo { get; set; }
    public string data { get; set; }
    public bool? feriado { get; set; }
    public bool? fixo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
