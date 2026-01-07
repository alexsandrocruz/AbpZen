using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Cursos.Curso.Dtos;

[Serializable]
public class CursoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
