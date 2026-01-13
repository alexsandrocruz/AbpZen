using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advAgeTiposCompromissos.Dtos;

[Serializable]
public class advAgeTiposCompromissosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idTipoCompromisso { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? recebimentoProcesso { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
