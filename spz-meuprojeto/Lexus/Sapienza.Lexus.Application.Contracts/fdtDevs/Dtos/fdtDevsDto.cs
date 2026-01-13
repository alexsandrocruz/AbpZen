using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fdtDevs.Dtos;

[Serializable]
public class fdtDevsDto : FullAuditedEntityDto<Guid>
{
    public int? idDev { get; set; }
    public string pacote { get; set; }
    public string descricao { get; set; }
    public bool? pendente { get; set; }
    public bool? aprovado { get; set; }
    public bool? reprovado { get; set; }
    public bool? finalizado { get; set; }
    public string comentariosRevisor { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string incluidoPor { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string alteradoPor { get; set; }
    public bool? ativo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
