using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Edital.Dtos;

[Serializable]
public class EditalGetListInput : PagedAndSortedResultRequestDto
{
    public string? Objeto { get; set; }
    public DateTime? Data { get; set; }
    public decimal? Valor { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
