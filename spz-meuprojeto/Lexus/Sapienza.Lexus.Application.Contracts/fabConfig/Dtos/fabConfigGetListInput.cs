using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabConfig.Dtos;

[Serializable]
public class fabConfigGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idConfig { get; set; }
    public string? imagemLogin { get; set; }
    public string? imagemLoginCentral { get; set; }
    public string? imagemLoginTickets { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public double? precoCombustivel { get; set; }
    public string? dataBloqueioFinanceiro { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
