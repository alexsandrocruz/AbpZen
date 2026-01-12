using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Turma.Dtos;

[Serializable]
public class TurmaGetListInput : PagedAndSortedResultRequestDto
{
    public string? Nome { get; set; }
    public string? Codigo { get; set; }
    public int? AnoLetivo { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
