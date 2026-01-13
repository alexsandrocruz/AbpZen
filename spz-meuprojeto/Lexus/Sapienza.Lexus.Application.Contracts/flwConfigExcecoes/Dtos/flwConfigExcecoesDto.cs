using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.flwConfigExcecoes.Dtos;

[Serializable]
public class flwConfigExcecoesDto : FullAuditedEntityDto<Guid>
{
    public int? idConfig { get; set; }
    public string tipoMarcacoes { get; set; }
    public int idHistoricoTipo { get; set; }
    public string data { get; set; }
    public int? qtde { get; set; }
    public int? manhaQtde { get; set; }
    public int? tardeQtde { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
