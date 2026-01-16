using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPreCheckListsGrupos.Dtos;

[Serializable]
public class advPreCheckListsGruposDto : FullAuditedEntityDto<Guid>
{
    public int? idGrupo { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? advPreCheckListsId { get; set; }
    public string? advPreCheckListsDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
