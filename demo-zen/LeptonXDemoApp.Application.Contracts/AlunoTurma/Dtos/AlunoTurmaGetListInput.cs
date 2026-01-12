using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.AlunoTurma.Dtos;

[Serializable]
public class AlunoTurmaGetListInput : PagedAndSortedResultRequestDto
{
    public DateTime? DataMatricula { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? AlunoId { get; set; }
    public Guid? TurmaId { get; set; }
}
