using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProfissionaisEstados.Dtos;

[Serializable]
public class advProfissionaisEstadosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idProfissionalEstado { get; set; }
    public int? idProfissional { get; set; }
    public string? estado { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
