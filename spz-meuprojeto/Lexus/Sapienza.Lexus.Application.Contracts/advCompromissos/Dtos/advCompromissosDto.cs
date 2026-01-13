using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCompromissos.Dtos;

[Serializable]
public class advCompromissosDto : FullAuditedEntityDto<Guid>
{
    public int? idCompromisso { get; set; }
    public int idTipoCompromisso { get; set; }
    public int? idProcesso { get; set; }
    public string dataPublicacao { get; set; }
    public string dataPrazoInterno { get; set; }
    public string dataPrazoFatal { get; set; }
    public string descricao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string incluidoPor { get; set; }
    public string alteradoPor { get; set; }
    public int? idAgendamentoINSS { get; set; }
    public bool? pauta { get; set; }
    public int? pautaIdUsuarioResp { get; set; }
    public bool? pautaRespAceite { get; set; }
    public int? horarioInicial { get; set; }
    public int? horarioFinal { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
