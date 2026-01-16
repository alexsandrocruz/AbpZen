using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.opoOportunidades.Dtos;

[Serializable]
public class opoOportunidadesDto : FullAuditedEntityDto<Guid>
{
    public int? idOportunidade { get; set; }
    public int idCliente { get; set; }
    public Guid IdentityUserId { get; set; }
    public int idTipo { get; set; }
    public int idSituacao { get; set; }
    public string titulo { get; set; }
    public string numero { get; set; }
    public string dataInicio { get; set; }
    public string dataEstimada { get; set; }
    public double? valorEstimado { get; set; }
    public string comentario { get; set; }
    public bool? aproveitada { get; set; }
    public string aproveitadaData { get; set; }
    public bool? cancelada { get; set; }
    public string canceladaMotivo { get; set; }
    public string canceladaData { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? indicadorCanceladoVisto { get; set; }
    public double? valorEstimadoMensal { get; set; }
    public bool? deProcesso { get; set; }
    public string aproveitadaMotivo { get; set; }
    public int? numeroProcesso { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? opoOrcamentosId { get; set; }
    public string? opoOrcamentosDisplayName { get; set; }
    public Guid flwFollowsId { get; set; }
    public string? flwFollowsDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
