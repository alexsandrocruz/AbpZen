using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProcessosDadosHerdeiros.Dtos;

[Serializable]
public class advProcessosDadosHerdeirosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idHerdeiro { get; set; }
    public int? idProcesso { get; set; }
    public int? sequencia { get; set; }
    public int? bancarioBancoId { get; set; }
    public string? bancarioTipoConta { get; set; }
    public string? bancarioAgencia { get; set; }
    public string? bancarioConta { get; set; }
    public string? bancarioFavorecido { get; set; }
    public string? bancarioCpf { get; set; }
    public double? bancarioPerc { get; set; }
    public double? bancarioTarifa { get; set; }
    public string? bancarioTarifaParcelas { get; set; }
    public int? idHonorario { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
