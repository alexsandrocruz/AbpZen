using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Cursos.Turma.Dtos;

[Serializable]
public class TurmaGetListInput : PagedAndSortedResultRequestDto
{
    public string? Nome { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
