using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.usuPermissoes.Dtos;

[Serializable]
public class usuPermissoesDto : FullAuditedEntityDto<Guid>
{
    public int? idUsuarioPermissao { get; set; }
    public int? idUsuario { get; set; }
    public int? idPermissao { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
