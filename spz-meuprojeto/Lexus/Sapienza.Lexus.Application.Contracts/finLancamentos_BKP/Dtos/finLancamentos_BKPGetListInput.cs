using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finLancamentos_BKP.Dtos;

[Serializable]
public class finLancamentos_BKPGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idLancamento { get; set; }
    public int? idConta { get; set; }
    public int? idPlanoConta { get; set; }
    public int? idCentroCusto { get; set; }
    public string? operacao { get; set; }
    public int? idForma { get; set; }
    public string? modulo { get; set; }
    public int? idCadastro { get; set; }
    public int? idPedido { get; set; }
    public string? descricao { get; set; }
    public string? nrDocumento { get; set; }
    public double? valor { get; set; }
    public string? dataEmissao { get; set; }
    public string? dataVencimento { get; set; }
    public string? dataQuitacao { get; set; }
    public bool? quitado { get; set; }
    public bool? recorrente { get; set; }
    public string? recorrenteChave { get; set; }
    public bool? previsao { get; set; }
    public bool? cobrancaEnviada { get; set; }
    public bool? parcelado { get; set; }
    public int? identificacao { get; set; }
    public string? observacao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idUsuarioInclusao { get; set; }
    public int? idUsuarioAlteracao { get; set; }
    public int? parcela { get; set; }
    public int? parcelaMaxima { get; set; }
    public string? dataVencimentoOriginal { get; set; }
    public int? pagtoLiberado { get; set; }
    public string? dataParaPrevisao { get; set; }
    public bool? recorrenteVencendoVisto { get; set; }
    public bool? recebimentoFuturo { get; set; }
    public bool? recebimentoFuturoRel { get; set; }
    public int? idTerceiro { get; set; }
    public string? arquivoDocumento { get; set; }
    public string? arquivoComprovante { get; set; }
    public int? idClientePagar { get; set; }
    public int? idProcessoPagar { get; set; }
    public int? idArea { get; set; }
    public string? identificacaoPagar { get; set; }
    public string? identificacaoPagar2 { get; set; }
    public string? arquivoDocumento2 { get; set; }
    public string? arquivoComprovante2 { get; set; }
    public bool? verba { get; set; }
    public string? verbaDataDe { get; set; }
    public string? verbaDataAte { get; set; }
    public string? verbaEstado { get; set; }
    public string? verbaCidade { get; set; }
    public int? idCentroResultado { get; set; }
    public bool? secundaria { get; set; }
    public bool? geradoPeloProcesso { get; set; }
    public int? sequenciaHerdeiro { get; set; }
    public int? idUnidade { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
