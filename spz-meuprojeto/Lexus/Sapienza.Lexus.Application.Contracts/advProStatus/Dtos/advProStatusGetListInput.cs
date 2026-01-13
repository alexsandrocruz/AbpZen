using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProStatus.Dtos;

[Serializable]
public class advProStatusGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idStatus { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
