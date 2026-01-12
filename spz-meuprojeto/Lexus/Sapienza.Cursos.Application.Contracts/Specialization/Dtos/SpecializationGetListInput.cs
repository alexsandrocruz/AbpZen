using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Cursos.Specialization.Dtos;

[Serializable]
public class SpecializationGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Name { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
