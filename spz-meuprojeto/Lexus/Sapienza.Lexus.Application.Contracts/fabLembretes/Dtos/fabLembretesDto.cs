using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabLembretes.Dtos;

[Serializable]
public class fabLembretesDto : FullAuditedEntityDto<Guid>
{
    public int? idLembrete { get; set; }
    public int? idUsuario { get; set; }
    public string mensagem { get; set; }
    public string destino { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string incluidoPor { get; set; }
    public bool? lido { get; set; }
    public string tipo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
