using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPreMetas.Dtos;

[Serializable]
public class advPreMetasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idMeta { get; set; }
    public string? tipo { get; set; }
    public Guid? ResponsibleUserId { get; set; }
    public int? idEscritorio { get; set; }
    public int? qtde { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
