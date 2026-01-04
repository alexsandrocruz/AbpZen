using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using LeptonXDemoApp.AlunoTurma.Dtos;

namespace LeptonXDemoApp.Turma.Dtos;

[Serializable]
public class TurmaDto : FullAuditedEntityDto<Guid>
{
    public string Nome { get; set; }
    public string Codigo { get; set; }
    public int AnoLetivo { get; set; }
    public int Semestre { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<AlunoTurmaDto> AlunoTurmas { get; set; }
}
