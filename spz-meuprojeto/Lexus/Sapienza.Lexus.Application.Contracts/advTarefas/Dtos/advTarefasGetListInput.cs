using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advTarefas.Dtos;

[Serializable]
public class advTarefasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idTarefa { get; set; }
    public int? idTipoTarefa { get; set; }
    public int? idCompromisso { get; set; }
    public int? idProcesso { get; set; }
    public string? dataCadastro { get; set; }
    public string? dataParaFinalizacao { get; set; }
    public string? descricao { get; set; }
    public Guid? ResponsibleUserId { get; set; }
    public int? idExecutor { get; set; }
    public bool? finalizado { get; set; }
    public DateTime? tsFinalizacao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? incluidoPor { get; set; }
    public string? alteradoPor { get; set; }
    public bool? agendada { get; set; }
    public int? horarioInicial { get; set; }
    public int? horarioFinal { get; set; }
    public string? onde { get; set; }
    public int? idCliente { get; set; }
    public int? idUsuarioFinalizou { get; set; }
    public int? lembreteQuandoFinalizarPara { get; set; }
    public bool? tecnica { get; set; }
    public bool? coletivoOriginal { get; set; }
    public int? coletivoIdOriginal { get; set; }
    public int? coletivoIdCliente { get; set; }
    public bool? pauta { get; set; }
    public int? pautaIdUsuarioResp { get; set; }
    public bool? pautaRespAceite { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
