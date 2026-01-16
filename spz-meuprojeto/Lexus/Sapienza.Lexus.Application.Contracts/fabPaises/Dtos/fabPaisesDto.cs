using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabPaises.Dtos;

[Serializable]
public class fabPaisesDto : FullAuditedEntityDto<Guid>
{
    public int? idPais { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? fabEstadosId { get; set; }
    public string? fabEstadosDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
