using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProcessos.ViewModels;

public class CreateadvProcessosViewModel
{
    [Display(Name = "advProcessos:idProcesso")]
    public int? idProcesso { get; set; }
    [Display(Name = "advProcessos:idCliente")]
    public int? idCliente { get; set; }
    [Display(Name = "advProcessos:idUsuarioInclusao")]
    public int? idUsuarioInclusao { get; set; }
    [Display(Name = "advProcessos:idEscritorioOrigem")]
    public int? idEscritorioOrigem { get; set; }
    [Display(Name = "advProcessos:idEscritorioResponsavel")]
    public int? idEscritorioResponsavel { get; set; }
    [Display(Name = "advProcessos:idAutorPeticao")]
    public int? idAutorPeticao { get; set; }
    [Display(Name = "advProcessos:idResponsavel")]
    public int? idResponsavel { get; set; }
    [Display(Name = "advProcessos:sintese")]
    [TextArea(Rows = 3)]
    public string? sintese { get; set; }
    [Display(Name = "advProcessos:numero")]
    public string? numero { get; set; }
    [Display(Name = "advProcessos:dataDistribuicao")]
    public string? dataDistribuicao { get; set; }
    [Display(Name = "advProcessos:idStatus")]
    public int? idStatus { get; set; }
    [Display(Name = "advProcessos:idNatureza")]
    public int? idNatureza { get; set; }
    [Display(Name = "advProcessos:idTipo")]
    public int? idTipo { get; set; }
    [Display(Name = "advProcessos:estado")]
    public string? estado { get; set; }
    [Display(Name = "advProcessos:cidade")]
    public string? cidade { get; set; }
    [Display(Name = "advProcessos:idFase")]
    public int? idFase { get; set; }
    [Display(Name = "advProcessos:idRelevancia")]
    public int? idRelevancia { get; set; }
    [Display(Name = "advProcessos:idProbabilidade")]
    public int? idProbabilidade { get; set; }
    [Display(Name = "advProcessos:valorCausa")]
    public double? valorCausa { get; set; }
    [Display(Name = "advProcessos:valorHonorarios")]
    public double? valorHonorarios { get; set; }
    [Display(Name = "advProcessos:valorHonorariosTipo")]
    public string? valorHonorariosTipo { get; set; }
    [Display(Name = "advProcessos:observacoes")]
    public string? observacoes { get; set; }
    [Display(Name = "advProcessos:idSentenca")]
    public int? idSentenca { get; set; }
    [Display(Name = "advProcessos:dataSentenca")]
    public string? dataSentenca { get; set; }
    [Display(Name = "advProcessos:alvara")]
    public bool? alvara { get; set; }
    [Display(Name = "advProcessos:valorDeferido")]
    public double? valorDeferido { get; set; }
    [Display(Name = "advProcessos:dataEncerramento")]
    public string? dataEncerramento { get; set; }
    [Display(Name = "advProcessos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProcessos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProcessos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advProcessos:idOrgao")]
    public int? idOrgao { get; set; }
    [Display(Name = "advProcessos:idInstancia")]
    public int? idInstancia { get; set; }
    [Display(Name = "advProcessos:idVara")]
    public int? idVara { get; set; }
    [Display(Name = "advProcessos:recurso")]
    public bool? recurso { get; set; }
    [Display(Name = "advProcessos:recursoIdSentenca")]
    public int? recursoIdSentenca { get; set; }
    [Display(Name = "advProcessos:recursoDataSentenca")]
    public string? recursoDataSentenca { get; set; }
    [Display(Name = "advProcessos:alvaraPendente")]
    public bool? alvaraPendente { get; set; }
    [Display(Name = "advProcessos:alvaraPendenteDesde")]
    public string? alvaraPendenteDesde { get; set; }
    [Display(Name = "advProcessos:historicoNumeros")]
    [TextArea(Rows = 3)]
    public string? historicoNumeros { get; set; }
    [Display(Name = "advProcessos:recebeAcordo")]
    public bool? recebeAcordo { get; set; }
    [Display(Name = "advProcessos:recebeRPV")]
    public bool? recebeRPV { get; set; }
    [Display(Name = "advProcessos:recebePrecatorio")]
    public bool? recebePrecatorio { get; set; }
    [Display(Name = "advProcessos:recebeAlvara")]
    public bool? recebeAlvara { get; set; }
    [Display(Name = "advProcessos:recebeBanco")]
    public string? recebeBanco { get; set; }
    [Display(Name = "advProcessos:recebeDataLiberacao")]
    public string? recebeDataLiberacao { get; set; }
    [Display(Name = "advProcessos:pendOutrosValores")]
    public bool? pendOutrosValores { get; set; }
    [Display(Name = "advProcessos:pendOutrosValoresDataEncerramento")]
    public string? pendOutrosValoresDataEncerramento { get; set; }
    [Display(Name = "advProcessos:pendOutrosValoresDeferido")]
    public bool? pendOutrosValoresDeferido { get; set; }
    [Display(Name = "advProcessos:pendOutrosValoresValorDeferido")]
    public double? pendOutrosValoresValorDeferido { get; set; }
    [Display(Name = "advProcessos:acaoColetiva")]
    public bool? acaoColetiva { get; set; }
    [Display(Name = "advProcessos:temResponsavel")]
    public bool? temResponsavel { get; set; }
    [Display(Name = "advProcessos:nomeResponsavel")]
    public string? nomeResponsavel { get; set; }
    [Display(Name = "advProcessos:cpfResponsavel")]
    public string? cpfResponsavel { get; set; }
    [Display(Name = "advProcessos:imposto")]
    public double? imposto { get; set; }
    [Display(Name = "advProcessos:tarifa")]
    public double? tarifa { get; set; }
    [Display(Name = "advProcessos:complementoPositivo")]
    public double? complementoPositivo { get; set; }
    [Display(Name = "advProcessos:RPV")]
    public string? RPV { get; set; }
    [Display(Name = "advProcessos:bancarioBanco")]
    public string? bancarioBanco { get; set; }
    [Display(Name = "advProcessos:bancarioTipoConta")]
    public string? bancarioTipoConta { get; set; }
    [Display(Name = "advProcessos:bancarioAgencia")]
    public string? bancarioAgencia { get; set; }
    [Display(Name = "advProcessos:bancarioConta")]
    public string? bancarioConta { get; set; }
    [Display(Name = "advProcessos:bancarioFavorecido")]
    public string? bancarioFavorecido { get; set; }
    [Display(Name = "advProcessos:bancarioCpf")]
    public string? bancarioCpf { get; set; }
    [Display(Name = "advProcessos:nomeReu")]
    public string? nomeReu { get; set; }
    [Display(Name = "advProcessos:sucumbencia")]
    public double? sucumbencia { get; set; }
    [Display(Name = "advProcessos:idConta")]
    public int? idConta { get; set; }
    [Display(Name = "advProcessos:dataLiberacaoValorDeferido")]
    public string? dataLiberacaoValorDeferido { get; set; }
    [Display(Name = "advProcessos:boleto")]
    public bool? boleto { get; set; }
    [Display(Name = "advProcessos:precatorio")]
    public string? precatorio { get; set; }
    [Display(Name = "advProcessos:emitir")]
    public string? emitir { get; set; }
    [Display(Name = "advProcessos:emitido")]
    public bool? emitido { get; set; }
    [Display(Name = "advProcessos:formaRecebimento")]
    public string? formaRecebimento { get; set; }
    [Display(Name = "advProcessos:bancarioBancoId")]
    public int? bancarioBancoId { get; set; }
    [Display(Name = "advProcessos:dataPrevisaoRepasseCliente")]
    public string? dataPrevisaoRepasseCliente { get; set; }
    [Display(Name = "advProcessos:honorariosTextoFicha")]
    public string? honorariosTextoFicha { get; set; }
    [Display(Name = "advProcessos:nfComComplementoPositivo")]
    public bool? nfComComplementoPositivo { get; set; }
    [Display(Name = "advProcessos:valorHonorariosDestaque")]
    public double? valorHonorariosDestaque { get; set; }
    [Display(Name = "advProcessos:valorHonorariosDestaqueTipo")]
    public string? valorHonorariosDestaqueTipo { get; set; }
    [Display(Name = "advProcessos:dataPrevisaoHonorariosDestaque")]
    public string? dataPrevisaoHonorariosDestaque { get; set; }
    [Display(Name = "advProcessos:idContaPagar")]
    public int? idContaPagar { get; set; }
    [Display(Name = "advProcessos:bancarioPerc")]
    public double? bancarioPerc { get; set; }
    [Display(Name = "advProcessos:dataPrevistaClienteReceber")]
    public string? dataPrevistaClienteReceber { get; set; }
    [Display(Name = "advProcessos:sucumbenciaAdd")]
    public double? sucumbenciaAdd { get; set; }
    [Display(Name = "advProcessos:sucumbenciaAddData")]
    public string? sucumbenciaAddData { get; set; }
    [Display(Name = "advProcessos:sucumbenciaAddIdBanco")]
    public int? sucumbenciaAddIdBanco { get; set; }
    [Display(Name = "advProcessos:saldoDevedor")]
    public double? saldoDevedor { get; set; }
    [Display(Name = "advProcessos:herdeirosTipoValor")]
    public string? herdeirosTipoValor { get; set; }
    [Display(Name = "advProcessos:nrParcelasProcesso")]
    public int? nrParcelasProcesso { get; set; }
    [Display(Name = "advProcessos:nrParcelasSomenteSucumbencia")]
    public int? nrParcelasSomenteSucumbencia { get; set; }
    [Display(Name = "advProcessos:preProcesso")]
    public bool? preProcesso { get; set; }
    [Display(Name = "advProcessos:preProcessoPasta")]
    public string? preProcessoPasta { get; set; }
    [Display(Name = "advProcessos:preProcessoDataCriacao")]
    public string? preProcessoDataCriacao { get; set; }
    [Display(Name = "advProcessos:preProcessoDataPrevista")]
    public string? preProcessoDataPrevista { get; set; }
    [Display(Name = "advProcessos:preProcessoDataRealizada")]
    public string? preProcessoDataRealizada { get; set; }
    [Display(Name = "advProcessos:preProcessoIdStatus")]
    public int? preProcessoIdStatus { get; set; }
    [Display(Name = "advProcessos:tsConversao")]
    public DateTime? tsConversao { get; set; }
    [Display(Name = "advProcessos:perdido")]
    public bool? perdido { get; set; }
    [Display(Name = "advProcessos:tsPerdido")]
    public DateTime? tsPerdido { get; set; }
    [Display(Name = "advProcessos:idMotivoPerda")]
    public int? idMotivoPerda { get; set; }
    [Display(Name = "advProcessos:convertido")]
    public bool? convertido { get; set; }
    [Display(Name = "advProcessos:clientePrimeiraVez")]
    public bool? clientePrimeiraVez { get; set; }
    [Display(Name = "advProcessos:preProcessoIdTipo")]
    public int? preProcessoIdTipo { get; set; }
    [Display(Name = "advProcessos:tarifaParcelas")]
    public string? tarifaParcelas { get; set; }
    [Display(Name = "advProcessos:idOrigem")]
    public int? idOrigem { get; set; }
    [Display(Name = "advProcessos:dataEntrada")]
    public string? dataEntrada { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
