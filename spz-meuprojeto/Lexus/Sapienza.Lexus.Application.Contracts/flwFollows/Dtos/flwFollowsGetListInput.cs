using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.flwFollows.Dtos;

[Serializable]
public class flwFollowsGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idFollow { get; set; }
    public int? idCliente { get; set; }
    public int? idAcao { get; set; }
    public int? idTipo { get; set; }
    public Guid? IdentityUserId { get; set; }
    public string? data { get; set; }
    public int? horario { get; set; }
    public string? comentario { get; set; }
    public bool? finalizado { get; set; }
    public string? dataFinalizacao { get; set; }
    public int? horarioFinalizacao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idOportunidade { get; set; }
    public bool? chegou { get; set; }
    public DateTime? tsChegou { get; set; }
    public bool? naoComparecimento { get; set; }
    public bool? prioridade { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
