using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.usuPermissoes.Dtos;

[Serializable]
public class usuPermissoesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idUsuarioPermissao { get; set; }
    public int? idUsuario { get; set; }
    public int? idPermissao { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
