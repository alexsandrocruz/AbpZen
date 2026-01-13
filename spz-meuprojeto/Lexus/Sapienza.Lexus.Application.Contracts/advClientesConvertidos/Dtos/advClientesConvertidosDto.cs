using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesConvertidos.Dtos;

[Serializable]
public class advClientesConvertidosDto : FullAuditedEntityDto<Guid>
{
    public int? idRegistro { get; set; }
    public int idCliente { get; set; }
    public string data { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string convertidoPor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
