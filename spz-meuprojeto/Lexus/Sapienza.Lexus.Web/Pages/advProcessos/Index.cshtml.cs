using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProcessos;
using Sapienza.Lexus.advProcessos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProcessos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProcessosFilterInput advProcessosFilter { get; set; }
    
    private readonly IadvProcessosAppService _advProcessosAppService;

    public IndexModel(IadvProcessosAppService advProcessosAppService)
    {
        _advProcessosAppService = advProcessosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProcessosGetListInput input)
    {
        var result = await _advProcessosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProcessosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProcessosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idUsuarioInclusao")]
    public int? idUsuarioInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idEscritorioOrigem")]
    public int? idEscritorioOrigem { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idEscritorioResponsavel")]
    public int? idEscritorioResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idAutorPeticao")]
    public int? idAutorPeticao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idResponsavel")]
    public int? idResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:sintese")]
    public string? sintese { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:numero")]
    public string? numero { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:dataDistribuicao")]
    public string? dataDistribuicao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idStatus")]
    public int? idStatus { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idNatureza")]
    public int? idNatureza { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idTipo")]
    public int? idTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:estado")]
    public string? estado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:cidade")]
    public string? cidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idFase")]
    public int? idFase { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idRelevancia")]
    public int? idRelevancia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idProbabilidade")]
    public int? idProbabilidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:valorCausa")]
    public double? valorCausa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:valorHonorarios")]
    public double? valorHonorarios { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:valorHonorariosTipo")]
    public string? valorHonorariosTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:observacoes")]
    public string? observacoes { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idSentenca")]
    public int? idSentenca { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:dataSentenca")]
    public string? dataSentenca { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:alvara")]
    public bool? alvara { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:valorDeferido")]
    public double? valorDeferido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:dataEncerramento")]
    public string? dataEncerramento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idOrgao")]
    public int? idOrgao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idInstancia")]
    public int? idInstancia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idVara")]
    public int? idVara { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:recurso")]
    public bool? recurso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:recursoIdSentenca")]
    public int? recursoIdSentenca { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:recursoDataSentenca")]
    public string? recursoDataSentenca { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:alvaraPendente")]
    public bool? alvaraPendente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:alvaraPendenteDesde")]
    public string? alvaraPendenteDesde { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:historicoNumeros")]
    public string? historicoNumeros { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:recebeAcordo")]
    public bool? recebeAcordo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:recebeRPV")]
    public bool? recebeRPV { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:recebePrecatorio")]
    public bool? recebePrecatorio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:recebeAlvara")]
    public bool? recebeAlvara { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:recebeBanco")]
    public string? recebeBanco { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:recebeDataLiberacao")]
    public string? recebeDataLiberacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:pendOutrosValores")]
    public bool? pendOutrosValores { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:pendOutrosValoresDataEncerramento")]
    public string? pendOutrosValoresDataEncerramento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:pendOutrosValoresDeferido")]
    public bool? pendOutrosValoresDeferido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:pendOutrosValoresValorDeferido")]
    public double? pendOutrosValoresValorDeferido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:acaoColetiva")]
    public bool? acaoColetiva { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:temResponsavel")]
    public bool? temResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:nomeResponsavel")]
    public string? nomeResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:cpfResponsavel")]
    public string? cpfResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:imposto")]
    public double? imposto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:tarifa")]
    public double? tarifa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:complementoPositivo")]
    public double? complementoPositivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:RPV")]
    public string? RPV { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:bancarioBanco")]
    public string? bancarioBanco { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:bancarioTipoConta")]
    public string? bancarioTipoConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:bancarioAgencia")]
    public string? bancarioAgencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:bancarioConta")]
    public string? bancarioConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:bancarioFavorecido")]
    public string? bancarioFavorecido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:bancarioCpf")]
    public string? bancarioCpf { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:nomeReu")]
    public string? nomeReu { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:sucumbencia")]
    public double? sucumbencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idConta")]
    public int? idConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:dataLiberacaoValorDeferido")]
    public string? dataLiberacaoValorDeferido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:boleto")]
    public bool? boleto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:precatorio")]
    public string? precatorio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:emitir")]
    public string? emitir { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:emitido")]
    public bool? emitido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:formaRecebimento")]
    public string? formaRecebimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:bancarioBancoId")]
    public int? bancarioBancoId { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:dataPrevisaoRepasseCliente")]
    public string? dataPrevisaoRepasseCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:honorariosTextoFicha")]
    public string? honorariosTextoFicha { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:nfComComplementoPositivo")]
    public bool? nfComComplementoPositivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:valorHonorariosDestaque")]
    public double? valorHonorariosDestaque { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:valorHonorariosDestaqueTipo")]
    public string? valorHonorariosDestaqueTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:dataPrevisaoHonorariosDestaque")]
    public string? dataPrevisaoHonorariosDestaque { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idContaPagar")]
    public int? idContaPagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:bancarioPerc")]
    public double? bancarioPerc { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:dataPrevistaClienteReceber")]
    public string? dataPrevistaClienteReceber { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:sucumbenciaAdd")]
    public double? sucumbenciaAdd { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:sucumbenciaAddData")]
    public string? sucumbenciaAddData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:sucumbenciaAddIdBanco")]
    public int? sucumbenciaAddIdBanco { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:saldoDevedor")]
    public double? saldoDevedor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:herdeirosTipoValor")]
    public string? herdeirosTipoValor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:nrParcelasProcesso")]
    public int? nrParcelasProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:nrParcelasSomenteSucumbencia")]
    public int? nrParcelasSomenteSucumbencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:preProcesso")]
    public bool? preProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:preProcessoPasta")]
    public string? preProcessoPasta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:preProcessoDataCriacao")]
    public string? preProcessoDataCriacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:preProcessoDataPrevista")]
    public string? preProcessoDataPrevista { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:preProcessoDataRealizada")]
    public string? preProcessoDataRealizada { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:preProcessoIdStatus")]
    public int? preProcessoIdStatus { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:tsConversao")]
    public DateTime? tsConversao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:perdido")]
    public bool? perdido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:tsPerdido")]
    public DateTime? tsPerdido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idMotivoPerda")]
    public int? idMotivoPerda { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:convertido")]
    public bool? convertido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:clientePrimeiraVez")]
    public bool? clientePrimeiraVez { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:preProcessoIdTipo")]
    public int? preProcessoIdTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:tarifaParcelas")]
    public string? tarifaParcelas { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:idOrigem")]
    public int? idOrigem { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessos:dataEntrada")]
    public string? dataEntrada { get; set; }
}
