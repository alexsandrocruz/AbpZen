using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesModelos.Dtos;

[Serializable]
public class advClientesModelosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idModelo { get; set; }
    public string? titulo { get; set; }
    public string? conteudo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
