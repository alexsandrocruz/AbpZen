using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finLancamentos_BKP.ViewModels;

public class CreatefinLancamentos_BKPViewModel
{
    [Display(Name = "finLancamentos_BKP:idLancamento")]
    public int? idLancamento { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:idConta")]
    public int idConta { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:idPlanoConta")]
    public int idPlanoConta { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:idCentroCusto")]
    public int idCentroCusto { get; set; }
    [Display(Name = "finLancamentos_BKP:operacao")]
    public string? operacao { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:idForma")]
    public int idForma { get; set; }
    [Display(Name = "finLancamentos_BKP:modulo")]
    public string? modulo { get; set; }
    [Display(Name = "finLancamentos_BKP:idCadastro")]
    public int? idCadastro { get; set; }
    [Display(Name = "finLancamentos_BKP:idPedido")]
    public int? idPedido { get; set; }
    [Display(Name = "finLancamentos_BKP:descricao")]
    public string? descricao { get; set; }
    [Display(Name = "finLancamentos_BKP:nrDocumento")]
    public string? nrDocumento { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:valor")]
    public double valor { get; set; }
    [Display(Name = "finLancamentos_BKP:dataEmissao")]
    public string? dataEmissao { get; set; }
    [Display(Name = "finLancamentos_BKP:dataVencimento")]
    public string? dataVencimento { get; set; }
    [Display(Name = "finLancamentos_BKP:dataQuitacao")]
    public string? dataQuitacao { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:quitado")]
    public bool quitado { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:recorrente")]
    public bool recorrente { get; set; }
    [Display(Name = "finLancamentos_BKP:recorrenteChave")]
    public string? recorrenteChave { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:previsao")]
    public bool previsao { get; set; }
    [Display(Name = "finLancamentos_BKP:cobrancaEnviada")]
    public bool? cobrancaEnviada { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:parcelado")]
    public bool parcelado { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:identificacao")]
    public int identificacao { get; set; }
    [Display(Name = "finLancamentos_BKP:observacao")]
    public string? observacao { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:ativo")]
    public bool ativo { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:tsInclusao")]
    public DateTime tsInclusao { get; set; }
    [Display(Name = "finLancamentos_BKP:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finLancamentos_BKP:idUsuarioInclusao")]
    public int? idUsuarioInclusao { get; set; }
    [Display(Name = "finLancamentos_BKP:idUsuarioAlteracao")]
    public int? idUsuarioAlteracao { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:parcela")]
    public int parcela { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:parcelaMaxima")]
    public int parcelaMaxima { get; set; }
    [Display(Name = "finLancamentos_BKP:dataVencimentoOriginal")]
    public string? dataVencimentoOriginal { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:pagtoLiberado")]
    public int pagtoLiberado { get; set; }
    [Display(Name = "finLancamentos_BKP:dataParaPrevisao")]
    public string? dataParaPrevisao { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:recorrenteVencendoVisto")]
    public bool recorrenteVencendoVisto { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:recebimentoFuturo")]
    public bool recebimentoFuturo { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:recebimentoFuturoRel")]
    public bool recebimentoFuturoRel { get; set; }
    [Display(Name = "finLancamentos_BKP:idTerceiro")]
    public int? idTerceiro { get; set; }
    [Display(Name = "finLancamentos_BKP:arquivoDocumento")]
    public string? arquivoDocumento { get; set; }
    [Display(Name = "finLancamentos_BKP:arquivoComprovante")]
    public string? arquivoComprovante { get; set; }
    [Display(Name = "finLancamentos_BKP:idClientePagar")]
    public int? idClientePagar { get; set; }
    [Display(Name = "finLancamentos_BKP:idProcessoPagar")]
    public int? idProcessoPagar { get; set; }
    [Display(Name = "finLancamentos_BKP:idArea")]
    public int? idArea { get; set; }
    [Display(Name = "finLancamentos_BKP:identificacaoPagar")]
    public string? identificacaoPagar { get; set; }
    [Display(Name = "finLancamentos_BKP:identificacaoPagar2")]
    public string? identificacaoPagar2 { get; set; }
    [Display(Name = "finLancamentos_BKP:arquivoDocumento2")]
    public string? arquivoDocumento2 { get; set; }
    [Display(Name = "finLancamentos_BKP:arquivoComprovante2")]
    public string? arquivoComprovante2 { get; set; }
    [Display(Name = "finLancamentos_BKP:verba")]
    public bool? verba { get; set; }
    [Display(Name = "finLancamentos_BKP:verbaDataDe")]
    public string? verbaDataDe { get; set; }
    [Display(Name = "finLancamentos_BKP:verbaDataAte")]
    public string? verbaDataAte { get; set; }
    [Display(Name = "finLancamentos_BKP:verbaEstado")]
    public string? verbaEstado { get; set; }
    [Display(Name = "finLancamentos_BKP:verbaCidade")]
    public string? verbaCidade { get; set; }
    [Display(Name = "finLancamentos_BKP:idCentroResultado")]
    public int? idCentroResultado { get; set; }
    [Required]
    [Display(Name = "finLancamentos_BKP:secundaria")]
    public bool secundaria { get; set; }
    [Display(Name = "finLancamentos_BKP:geradoPeloProcesso")]
    public bool? geradoPeloProcesso { get; set; }
    [Display(Name = "finLancamentos_BKP:sequenciaHerdeiro")]
    public int? sequenciaHerdeiro { get; set; }
    [Display(Name = "finLancamentos_BKP:idUnidade")]
    public int? idUnidade { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
