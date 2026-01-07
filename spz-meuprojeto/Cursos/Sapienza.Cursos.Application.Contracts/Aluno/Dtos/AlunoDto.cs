using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Cursos.Aluno.Dtos;

[Serializable]
public class AlunoDto : FullAuditedEntityDto<Guid>
{
    public string Nome { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
