using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProcessosAlteracoes.Dtos;

[Serializable]
public class advProcessosAlteracoesDto : FullAuditedEntityDto<Guid>
{
    public int? idProcessoAlteracao { get; set; }
    public int idProcesso { get; set; }
    public Guid IdentityUserId { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string texto { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
