using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.finLancamentos_BKP.Dtos;

[Serializable]
public class CreateUpdatefinLancamentos_BKPDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idLancamento { get; set; }
    [Required]
    public int idConta { get; set; }
    [Required]
    public int idPlanoConta { get; set; }
    [Required]
    public int idCentroCusto { get; set; }
    public string operacao { get; set; }
    [Required]
    public int idForma { get; set; }
    public string modulo { get; set; }
    public int? idCadastro { get; set; }
    public int? idPedido { get; set; }
    public string descricao { get; set; }
    public string nrDocumento { get; set; }
    [Required]
    public double valor { get; set; }
    public string dataEmissao { get; set; }
    public string dataVencimento { get; set; }
    public string dataQuitacao { get; set; }
    [Required]
    public bool quitado { get; set; }
    [Required]
    public bool recorrente { get; set; }
    public string recorrenteChave { get; set; }
    [Required]
    public bool previsao { get; set; }
    public bool? cobrancaEnviada { get; set; }
    [Required]
    public bool parcelado { get; set; }
    [Required]
    public int identificacao { get; set; }
    public string observacao { get; set; }
    [Required]
    public bool ativo { get; set; }
    [Required]
    public DateTime tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idUsuarioInclusao { get; set; }
    public int? idUsuarioAlteracao { get; set; }
    [Required]
    public int parcela { get; set; }
    [Required]
    public int parcelaMaxima { get; set; }
    public string dataVencimentoOriginal { get; set; }
    [Required]
    public int pagtoLiberado { get; set; }
    public string dataParaPrevisao { get; set; }
    [Required]
    public bool recorrenteVencendoVisto { get; set; }
    [Required]
    public bool recebimentoFuturo { get; set; }
    [Required]
    public bool recebimentoFuturoRel { get; set; }
    public int? idTerceiro { get; set; }
    public string arquivoDocumento { get; set; }
    public string arquivoComprovante { get; set; }
    public int? idClientePagar { get; set; }
    public int? idProcessoPagar { get; set; }
    public int? idArea { get; set; }
    public string identificacaoPagar { get; set; }
    public string identificacaoPagar2 { get; set; }
    public string arquivoDocumento2 { get; set; }
    public string arquivoComprovante2 { get; set; }
    public bool? verba { get; set; }
    public string verbaDataDe { get; set; }
    public string verbaDataAte { get; set; }
    public string verbaEstado { get; set; }
    public string verbaCidade { get; set; }
    public int? idCentroResultado { get; set; }
    [Required]
    public bool secundaria { get; set; }
    public bool? geradoPeloProcesso { get; set; }
    public int? sequenciaHerdeiro { get; set; }
    public int? idUnidade { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
