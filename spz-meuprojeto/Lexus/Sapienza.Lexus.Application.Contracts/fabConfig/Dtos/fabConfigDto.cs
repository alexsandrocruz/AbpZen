using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabConfig.Dtos;

[Serializable]
public class fabConfigDto : FullAuditedEntityDto<Guid>
{
    public int? idConfig { get; set; }
    public string imagemLogin { get; set; }
    public string imagemLoginCentral { get; set; }
    public string imagemLoginTickets { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public double? precoCombustivel { get; set; }
    public string dataBloqueioFinanceiro { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
