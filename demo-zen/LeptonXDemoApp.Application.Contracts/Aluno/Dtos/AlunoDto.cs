using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using LeptonXDemoApp.AlunoTurma.Dtos;

namespace LeptonXDemoApp.Aluno.Dtos;

[Serializable]
public class AlunoDto : FullAuditedEntityDto<Guid>
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Matricula { get; set; }
    public DateTime? DataNascimento { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<AlunoTurmaDto> AlunoTurmas { get; set; }
}
