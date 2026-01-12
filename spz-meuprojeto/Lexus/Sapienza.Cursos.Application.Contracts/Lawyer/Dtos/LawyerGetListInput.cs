using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Cursos.Lawyer.Dtos;

[Serializable]
public class LawyerGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? FullName { get; set; }
    public string? PreferredName { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
