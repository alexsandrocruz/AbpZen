using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCliTiposArquivos.Dtos;

[Serializable]
public class advCliTiposArquivosDto : FullAuditedEntityDto<Guid>
{
    public int? idTipoArquivo { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string pasta { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? advClientesArquivosId { get; set; }
    public string? advClientesArquivosDisplayName { get; set; }
    public Guid? advClientesChecklistId { get; set; }
    public string? advClientesChecklistDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
