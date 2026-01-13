using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finRateiosPadrao.Dtos;

[Serializable]
public class finRateiosPadraoDto : FullAuditedEntityDto<Guid>
{
    public int? idPadrao { get; set; }
    public int idUnidade { get; set; }
    public int idCentroResultado { get; set; }
    public double porcentagem { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
