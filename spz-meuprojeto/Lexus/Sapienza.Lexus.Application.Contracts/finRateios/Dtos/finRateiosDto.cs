using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finRateios.Dtos;

[Serializable]
public class finRateiosDto : FullAuditedEntityDto<Guid>
{
    public int? idRateio { get; set; }
    public int idLancamento { get; set; }
    public int idCentroCusto { get; set; }
    public int? idCentroResultado { get; set; }
    public double? percentualCC { get; set; }
    public double? percentualCR { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idUnidade { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
