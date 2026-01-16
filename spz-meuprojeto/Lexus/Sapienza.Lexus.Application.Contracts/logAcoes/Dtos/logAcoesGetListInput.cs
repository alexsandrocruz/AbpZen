using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.logAcoes.Dtos;

[Serializable]
public class logAcoesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idLog { get; set; }
    public string? area { get; set; }
    public string? acao { get; set; }
    public string? usuario { get; set; }
    public string? motivo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public int? idCliente { get; set; }
    public int? idProcesso { get; set; }
    public int? idCompromisso { get; set; }
    public int? idTarefa { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? logCamposId { get; set; }
}
