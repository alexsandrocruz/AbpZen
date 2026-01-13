using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advRevisaoDocumentos.Dtos;

[Serializable]
public class advRevisaoDocumentosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idRevisao { get; set; }
    public int? idUsuarioSolicitante { get; set; }
    public int? idUsuarioRevisor { get; set; }
    public string? localRede { get; set; }
    public bool? pendente { get; set; }
    public bool? aprovado { get; set; }
    public bool? reprovado { get; set; }
    public bool? finalizado { get; set; }
    public string? comentariosSolicitante { get; set; }
    public string? comentariosRevisor { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string? incluidoPor { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? alteradoPor { get; set; }
    public bool? ativo { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
