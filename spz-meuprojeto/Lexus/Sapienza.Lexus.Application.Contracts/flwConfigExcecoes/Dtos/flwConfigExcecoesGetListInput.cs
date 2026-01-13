using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.flwConfigExcecoes.Dtos;

[Serializable]
public class flwConfigExcecoesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idConfig { get; set; }
    public string? tipoMarcacoes { get; set; }
    public int? idHistoricoTipo { get; set; }
    public string? data { get; set; }
    public int? qtde { get; set; }
    public int? manhaQtde { get; set; }
    public int? tardeQtde { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
