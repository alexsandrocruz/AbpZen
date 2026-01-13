using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProcessosClientes.Dtos;

[Serializable]
public class advProcessosClientesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idProcessoCliente { get; set; }
    public int? idProcesso { get; set; }
    public int? idCliente { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
