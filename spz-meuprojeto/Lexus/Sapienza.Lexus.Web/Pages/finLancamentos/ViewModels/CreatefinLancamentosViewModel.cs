using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finLancamentos.ViewModels;

public class CreatefinLancamentosViewModel
{
    [Display(Name = "finLancamentos:idLancamento")]
    public int? idLancamento { get; set; }
    [Required]
    [Display(Name = "finLancamentos:idConta")]
    public int idConta { get; set; }
    [Required]
    [Display(Name = "finLancamentos:idPlanoConta")]
    public int idPlanoConta { get; set; }
    [Required]
    [Display(Name = "finLancamentos:idCentroCusto")]
    public int idCentroCusto { get; set; }
    [Display(Name = "finLancamentos:operacao")]
    public string? operacao { get; set; }
    [Required]
    [Display(Name = "finLancamentos:idForma")]
    public int idForma { get; set; }
    [Display(Name = "finLancamentos:modulo")]
    public string? modulo { get; set; }
    [Display(Name = "finLancamentos:idCadastro")]
    public int? idCadastro { get; set; }
    [Display(Name = "finLancamentos:idPedido")]
    public int? idPedido { get; set; }
    [Display(Name = "finLancamentos:descricao")]
    public string? descricao { get; set; }
    [Display(Name = "finLancamentos:nrDocumento")]
    public string? nrDocumento { get; set; }
    [Display(Name = "finLancamentos:valor")]
    public double? valor { get; set; }
    [Display(Name = "finLancamentos:dataEmissao")]
    public string? dataEmissao { get; set; }
    [Display(Name = "finLancamentos:dataVencimento")]
    public string? dataVencimento { get; set; }
    [Display(Name = "finLancamentos:dataQuitacao")]
    public string? dataQuitacao { get; set; }
    [Display(Name = "finLancamentos:quitado")]
    public bool? quitado { get; set; }
    [Display(Name = "finLancamentos:recorrente")]
    public bool? recorrente { get; set; }
    [Display(Name = "finLancamentos:recorrenteChave")]
    public string? recorrenteChave { get; set; }
    [Display(Name = "finLancamentos:previsao")]
    public bool? previsao { get; set; }
    [Display(Name = "finLancamentos:cobrancaEnviada")]
    public bool? cobrancaEnviada { get; set; }
    [Display(Name = "finLancamentos:parcelado")]
    public bool? parcelado { get; set; }
    [Display(Name = "finLancamentos:identificacao")]
    public int? identificacao { get; set; }
    [Display(Name = "finLancamentos:observacao")]
    public string? observacao { get; set; }
    [Display(Name = "finLancamentos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finLancamentos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finLancamentos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finLancamentos:idUsuarioInclusao")]
    public int? idUsuarioInclusao { get; set; }
    [Display(Name = "finLancamentos:idUsuarioAlteracao")]
    public int? idUsuarioAlteracao { get; set; }
    [Display(Name = "finLancamentos:parcela")]
    public int? parcela { get; set; }
    [Display(Name = "finLancamentos:parcelaMaxima")]
    public int? parcelaMaxima { get; set; }
    [Display(Name = "finLancamentos:dataVencimentoOriginal")]
    public string? dataVencimentoOriginal { get; set; }
    [Display(Name = "finLancamentos:pagtoLiberado")]
    public int? pagtoLiberado { get; set; }
    [Display(Name = "finLancamentos:dataParaPrevisao")]
    public string? dataParaPrevisao { get; set; }
    [Display(Name = "finLancamentos:recorrenteVencendoVisto")]
    public bool? recorrenteVencendoVisto { get; set; }
    [Display(Name = "finLancamentos:recebimentoFuturo")]
    public bool? recebimentoFuturo { get; set; }
    [Display(Name = "finLancamentos:recebimentoFuturoRel")]
    public bool? recebimentoFuturoRel { get; set; }
    [Display(Name = "finLancamentos:idTerceiro")]
    public int? idTerceiro { get; set; }
    [Display(Name = "finLancamentos:arquivoDocumento")]
    public string? arquivoDocumento { get; set; }
    [Display(Name = "finLancamentos:arquivoComprovante")]
    public string? arquivoComprovante { get; set; }
    [Display(Name = "finLancamentos:idClientePagar")]
    public int? idClientePagar { get; set; }
    [Display(Name = "finLancamentos:idProcessoPagar")]
    public int? idProcessoPagar { get; set; }
    [Display(Name = "finLancamentos:idArea")]
    public int? idArea { get; set; }
    [Display(Name = "finLancamentos:identificacaoPagar")]
    public string? identificacaoPagar { get; set; }
    [Display(Name = "finLancamentos:identificacaoPagar2")]
    public string? identificacaoPagar2 { get; set; }
    [Display(Name = "finLancamentos:arquivoDocumento2")]
    public string? arquivoDocumento2 { get; set; }
    [Display(Name = "finLancamentos:arquivoComprovante2")]
    public string? arquivoComprovante2 { get; set; }
    [Display(Name = "finLancamentos:verba")]
    public bool? verba { get; set; }
    [Display(Name = "finLancamentos:verbaDataDe")]
    public string? verbaDataDe { get; set; }
    [Display(Name = "finLancamentos:verbaDataAte")]
    public string? verbaDataAte { get; set; }
    [Display(Name = "finLancamentos:verbaEstado")]
    public string? verbaEstado { get; set; }
    [Display(Name = "finLancamentos:verbaCidade")]
    public string? verbaCidade { get; set; }
    [Display(Name = "finLancamentos:idCentroResultado")]
    public int? idCentroResultado { get; set; }
    [Display(Name = "finLancamentos:secundaria")]
    public bool? secundaria { get; set; }
    [Display(Name = "finLancamentos:geradoPeloProcesso")]
    public bool? geradoPeloProcesso { get; set; }
    [Display(Name = "finLancamentos:sequenciaHerdeiro")]
    public int? sequenciaHerdeiro { get; set; }
    [Display(Name = "finLancamentos:idUnidade")]
    public int? idUnidade { get; set; }
    [Display(Name = "finLancamentos:rateioFeito")]
    public bool? rateioFeito { get; set; }
    [Display(Name = "finLancamentos:naoAbatePagtoDoSaldoDoCliente")]
    public bool? naoAbatePagtoDoSaldoDoCliente { get; set; }
    [Display(Name = "finLancamentos:idHonorario")]
    public int? idHonorario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
