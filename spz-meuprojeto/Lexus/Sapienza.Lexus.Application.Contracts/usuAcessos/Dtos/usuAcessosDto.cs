using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.usuAcessos.Dtos;

[Serializable]
public class usuAcessosDto : FullAuditedEntityDto<Guid>
{
    public int? idAcesso { get; set; }
    public Guid IdentityUserId { get; set; }
    public DateTime? data { get; set; }
    public string ip { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
