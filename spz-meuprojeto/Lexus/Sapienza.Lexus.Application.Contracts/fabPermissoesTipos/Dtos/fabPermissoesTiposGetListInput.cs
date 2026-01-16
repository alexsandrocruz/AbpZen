using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabPermissoesTipos.Dtos;

[Serializable]
public class fabPermissoesTiposGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idPermissaoTipo { get; set; }
    public string? descricao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? fabPermissoesId { get; set; }
}
