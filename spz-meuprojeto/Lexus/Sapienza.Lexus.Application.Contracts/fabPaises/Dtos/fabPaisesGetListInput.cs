using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabPaises.Dtos;

[Serializable]
public class fabPaisesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idPais { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? fabEstadosId { get; set; }
}
