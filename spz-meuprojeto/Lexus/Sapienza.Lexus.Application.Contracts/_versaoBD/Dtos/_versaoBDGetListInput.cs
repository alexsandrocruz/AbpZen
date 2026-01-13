using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus._versaoBD.Dtos;

[Serializable]
public class _versaoBDGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? id { get; set; }
    public string? arquivo { get; set; }
    public DateTime? dataAplicacao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
