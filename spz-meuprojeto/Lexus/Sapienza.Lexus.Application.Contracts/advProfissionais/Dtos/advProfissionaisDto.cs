using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProfissionais.Dtos;

[Serializable]
public class advProfissionaisDto : FullAuditedEntityDto<Guid>
{
    public int? idProfissional { get; set; }
    public Guid IdentityUserId { get; set; }
    public string nome { get; set; }
    public string email { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? advProfissionaisEstadosId { get; set; }
    public string? advProfissionaisEstadosDisplayName { get; set; }
    public Guid? advProfissionaisNaturezasId { get; set; }
    public string? advProfissionaisNaturezasDisplayName { get; set; }
    public Guid? advVerbasId { get; set; }
    public string? advVerbasDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
