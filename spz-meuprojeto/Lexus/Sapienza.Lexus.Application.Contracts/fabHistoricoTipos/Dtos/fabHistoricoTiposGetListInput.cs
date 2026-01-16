using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabHistoricoTipos.Dtos;

[Serializable]
public class fabHistoricoTiposGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idHistoricoTipo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? tipoMarcacoes { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? flwConfigExcecoesId { get; set; }
    public Guid? flwGradeHorariosId { get; set; }
    public Guid? flwFollowsId { get; set; }
}
