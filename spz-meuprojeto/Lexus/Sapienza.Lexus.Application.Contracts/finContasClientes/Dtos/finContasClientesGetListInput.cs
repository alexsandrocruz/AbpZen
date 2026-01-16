using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finContasClientes.Dtos;

[Serializable]
public class finContasClientesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idContaCliente { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? cor { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
