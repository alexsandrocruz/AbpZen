using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPreArquivosStatus.Dtos;

[Serializable]
public class advPreArquivosStatusGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idStatus { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
