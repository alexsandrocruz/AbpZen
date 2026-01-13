using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPreMotivosPerda.Dtos;

[Serializable]
public class advPreMotivosPerdaGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idMotivo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
