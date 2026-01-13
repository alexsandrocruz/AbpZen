using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finRecibos.Dtos;

[Serializable]
public class finRecibosDto : FullAuditedEntityDto<Guid>
{
    public int? idRecibo { get; set; }
    public int? idLancamento { get; set; }
    public int? numero { get; set; }
    public string referente { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
