using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.logCampos.Dtos;

[Serializable]
public class logCamposDto : FullAuditedEntityDto<Guid>
{
    public int? idLogCampo { get; set; }
    public int idLog { get; set; }
    public string campo { get; set; }
    public string dadoAnterior { get; set; }
    public string dadoNovo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
