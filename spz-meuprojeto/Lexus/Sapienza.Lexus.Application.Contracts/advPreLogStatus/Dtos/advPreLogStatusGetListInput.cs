using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPreLogStatus.Dtos;

[Serializable]
public class advPreLogStatusGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idLog { get; set; }
    public int? idProcesso { get; set; }
    public int? idStatus { get; set; }
    public DateTime? tsInclusao { get; set; }
    public bool? conversao { get; set; }
    public DateTime? tsConversao { get; set; }
    public bool? perdido { get; set; }
    public DateTime? tsPerdido { get; set; }
    public int? diasCorridosDoAnterior { get; set; }
    public string? usuario { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
