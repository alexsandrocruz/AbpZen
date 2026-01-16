using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProMeritos.Dtos;

[Serializable]
public class advProMeritosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idMerito { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? beneficioINSS { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? advProcessosMeritosId { get; set; }
}
