using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.logAcoes.Dtos;

[Serializable]
public class logAcoesDto : FullAuditedEntityDto<Guid>
{
    public int? idLog { get; set; }
    public string area { get; set; }
    public string acao { get; set; }
    public string usuario { get; set; }
    public string motivo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public int? idCliente { get; set; }
    public int? idProcesso { get; set; }
    public int? idCompromisso { get; set; }
    public int? idTarefa { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
