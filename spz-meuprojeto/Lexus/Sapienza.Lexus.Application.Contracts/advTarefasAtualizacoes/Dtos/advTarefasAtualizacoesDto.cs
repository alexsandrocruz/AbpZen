using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advTarefasAtualizacoes.Dtos;

[Serializable]
public class advTarefasAtualizacoesDto : FullAuditedEntityDto<Guid>
{
    public int? idAtualizacaoTarefa { get; set; }
    public int? idTarefa { get; set; }
    public int? idCompromisso { get; set; }
    public string campo { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string dadoAnterior { get; set; }
    public int? idUsuario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
