using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPreStatus.Dtos;

[Serializable]
public class advPreStatusGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idStatus { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? ordem { get; set; }
    public bool? ultimo { get; set; }
    public int? diasMaxParado { get; set; }
    public int? idTipo { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
