using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProcessosAlteracoes.Dtos;

[Serializable]
public class advProcessosAlteracoesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idProcessoAlteracao { get; set; }
    public int? idProcesso { get; set; }
    public Guid? IdentityUserId { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string? texto { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
