using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesINSSStatus.Dtos;

[Serializable]
public class advClientesINSSStatusGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idStatus { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? advClientesINSSId { get; set; }
}
