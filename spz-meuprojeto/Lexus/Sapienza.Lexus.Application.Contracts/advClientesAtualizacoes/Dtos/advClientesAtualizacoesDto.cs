using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesAtualizacoes.Dtos;

[Serializable]
public class advClientesAtualizacoesDto : FullAuditedEntityDto<Guid>
{
    public int? idAtualizacao { get; set; }
    public int idCliente { get; set; }
    public string campo { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string dadoAnterior { get; set; }
    public Guid? IdentityUserId { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
