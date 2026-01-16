using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabCidades.Dtos;

[Serializable]
public class fabCidadesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idCidade { get; set; }
    public string? descricao { get; set; }
    public string? codigoIBGE { get; set; }
    public int? idEstado { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
