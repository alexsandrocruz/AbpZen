using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesAtualizacoes.Dtos;

[Serializable]
public class advClientesAtualizacoesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idAtualizacao { get; set; }
    public int? idCliente { get; set; }
    public string? campo { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? dadoAnterior { get; set; }
    public Guid? IdentityUserId { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
