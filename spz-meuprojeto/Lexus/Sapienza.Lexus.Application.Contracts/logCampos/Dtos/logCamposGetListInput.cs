using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.logCampos.Dtos;

[Serializable]
public class logCamposGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idLogCampo { get; set; }
    public int? idLog { get; set; }
    public string? campo { get; set; }
    public string? dadoAnterior { get; set; }
    public string? dadoNovo { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
