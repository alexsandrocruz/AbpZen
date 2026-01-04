using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Aluno.Dtos;

[Serializable]
public class AlunoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Matricula { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
