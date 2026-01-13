using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabPermissoes.Dtos;

[Serializable]
public class fabPermissoesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idPermissao { get; set; }
    public int? idPermissaoTipo { get; set; }
    public bool? modulo { get; set; }
    public string? descricao { get; set; }
    public string? varSession { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
