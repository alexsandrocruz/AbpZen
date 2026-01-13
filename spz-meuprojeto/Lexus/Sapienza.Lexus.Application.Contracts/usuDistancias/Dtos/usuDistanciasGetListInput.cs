using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.usuDistancias.Dtos;

[Serializable]
public class usuDistanciasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idDistancia { get; set; }
    public int? idUsuario { get; set; }
    public string? estado { get; set; }
    public string? cidade { get; set; }
    public int? km { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
