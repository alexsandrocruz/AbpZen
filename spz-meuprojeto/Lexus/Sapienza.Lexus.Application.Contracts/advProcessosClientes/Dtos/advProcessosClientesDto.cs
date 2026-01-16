using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProcessosClientes.Dtos;

[Serializable]
public class advProcessosClientesDto : FullAuditedEntityDto<Guid>
{
    public int? idProcessoCliente { get; set; }
    public int idProcesso { get; set; }
    public int idCliente { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
