using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesConvertidos.Dtos;

[Serializable]
public class advClientesConvertidosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idRegistro { get; set; }
    public int? idCliente { get; set; }
    public string? data { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string? convertidoPor { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
