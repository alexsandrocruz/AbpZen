using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesChecklist.Dtos;

[Serializable]
public class advClientesChecklistDto : FullAuditedEntityDto<Guid>
{
    public int? idClienteChecklist { get; set; }
    public int idCliente { get; set; }
    public int idTipoArquivo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
