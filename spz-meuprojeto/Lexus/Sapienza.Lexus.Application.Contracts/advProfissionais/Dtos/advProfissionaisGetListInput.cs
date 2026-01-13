using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProfissionais.Dtos;

[Serializable]
public class advProfissionaisGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idProfissional { get; set; }
    public int? idUsuario { get; set; }
    public string? nome { get; set; }
    public string? email { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
