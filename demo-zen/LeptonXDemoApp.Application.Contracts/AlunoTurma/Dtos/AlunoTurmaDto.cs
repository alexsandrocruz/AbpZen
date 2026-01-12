using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.AlunoTurma.Dtos;

[Serializable]
public class AlunoTurmaDto : FullAuditedEntityDto<Guid>
{
    public DateTime DataMatricula { get; set; }
    public string Situacao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid AlunoId { get; set; }
    public string? AlunoDisplayName { get; set; }
    public Guid TurmaId { get; set; }
    public string? TurmaDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
