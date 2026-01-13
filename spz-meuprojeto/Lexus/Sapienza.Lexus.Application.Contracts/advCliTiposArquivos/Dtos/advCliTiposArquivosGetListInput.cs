using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCliTiposArquivos.Dtos;

[Serializable]
public class advCliTiposArquivosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idTipoArquivo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? pasta { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
