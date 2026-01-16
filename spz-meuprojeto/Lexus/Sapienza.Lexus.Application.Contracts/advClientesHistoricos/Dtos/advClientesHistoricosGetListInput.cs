using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesHistoricos.Dtos;

[Serializable]
public class advClientesHistoricosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idHistorico { get; set; }
    public int? idCliente { get; set; }
    public int? idProcesso { get; set; }
    public Guid? IdentityUserId { get; set; }
    public int? idTipoHistorico { get; set; }
    public string? data { get; set; }
    public string? hora { get; set; }
    public string? ocorrencia { get; set; }
    public DateTime? tsInclusao { get; set; }
    public int? idOportunidade { get; set; }
    public string? depto { get; set; }
    public bool? prioritario { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
