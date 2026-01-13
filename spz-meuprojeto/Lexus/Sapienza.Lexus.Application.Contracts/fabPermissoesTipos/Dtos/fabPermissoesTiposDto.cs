using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabPermissoesTipos.Dtos;

[Serializable]
public class fabPermissoesTiposDto : FullAuditedEntityDto<Guid>
{
    public int? idPermissaoTipo { get; set; }
    public string descricao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
