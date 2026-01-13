using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabEstados.Dtos;

[Serializable]
public class fabEstadosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idEstado { get; set; }
    public string? sigla { get; set; }
    public string? descricao { get; set; }
    public int? idPais { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
