using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabPermissoes.Dtos;

[Serializable]
public class fabPermissoesDto : FullAuditedEntityDto<Guid>
{
    public int? idPermissao { get; set; }
    public int idPermissaoTipo { get; set; }
    public bool? modulo { get; set; }
    public string descricao { get; set; }
    public string varSession { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
