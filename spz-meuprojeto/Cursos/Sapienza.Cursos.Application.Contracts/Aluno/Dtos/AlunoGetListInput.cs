using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Cursos.Aluno.Dtos;

[Serializable]
public class AlunoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Nome { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
