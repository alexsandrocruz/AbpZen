using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.flwFollows.Dtos;

[Serializable]
public class flwFollowsDto : FullAuditedEntityDto<Guid>
{
    public int? idFollow { get; set; }
    public int idCliente { get; set; }
    public int idAcao { get; set; }
    public int idTipo { get; set; }
    public Guid IdentityUserId { get; set; }
    public string data { get; set; }
    public int? horario { get; set; }
    public string comentario { get; set; }
    public bool? finalizado { get; set; }
    public string dataFinalizacao { get; set; }
    public int? horarioFinalizacao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idOportunidade { get; set; }
    public bool? chegou { get; set; }
    public DateTime? tsChegou { get; set; }
    public bool? naoComparecimento { get; set; }
    public bool? prioridade { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
