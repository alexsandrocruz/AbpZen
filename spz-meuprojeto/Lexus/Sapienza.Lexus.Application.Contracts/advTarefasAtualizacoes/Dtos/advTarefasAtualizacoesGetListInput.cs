using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advTarefasAtualizacoes.Dtos;

[Serializable]
public class advTarefasAtualizacoesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idAtualizacaoTarefa { get; set; }
    public int? idTarefa { get; set; }
    public int? idCompromisso { get; set; }
    public string? campo { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? dadoAnterior { get; set; }
    public Guid? IdentityUserId { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
