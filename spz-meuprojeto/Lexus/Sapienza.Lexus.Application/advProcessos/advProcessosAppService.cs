using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProcessos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProcessos;

/// <summary>
/// Application service for advProcessos entity
/// </summary>
[Authorize(advProcessosPermissions.Default)]
public class advProcessosAppService :
    LexusAppService,
    IadvProcessosAppService
{
    private readonly IRepository<Sapienza.Lexus.advProcessos.advProcessos, Guid> _repository;

    public advProcessosAppService(
        IRepository<Sapienza.Lexus.advProcessos.advProcessos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProcessos by Id
    /// </summary>
    public virtual async Task<advProcessosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProcessos.advProcessos, advProcessosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProcessoses
    /// </summary>
    public virtual async Task<PagedResultDto<advProcessosDto>> GetListAsync(advProcessosGetListInput input)
    {
        var queryable = await _repository.GetQueryableAsync();

        // Apply filters
        queryable = ApplyFilters(queryable, input);

        // Apply default sorting (by CreationTime descending)
        queryable = queryable.OrderByDescending(e => e.CreationTime);

        // Get total count
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        // Apply paging
        queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);

        var entities = await AsyncExecuter.ToListAsync(queryable);
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProcessos.advProcessos>, List<advProcessosDto>>(entities);

        return new PagedResultDto<advProcessosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProcessos
    /// </summary>
    [Authorize(advProcessosPermissions.Create)]
    public virtual async Task<advProcessosDto> CreateAsync(CreateUpdateadvProcessosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProcessosDto, Sapienza.Lexus.advProcessos.advProcessos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessos.advProcessos, advProcessosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProcessos
    /// </summary>
    [Authorize(advProcessosPermissions.Update)]
    public virtual async Task<advProcessosDto> UpdateAsync(Guid id, CreateUpdateadvProcessosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProcessos.advProcessos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessos.advProcessos, advProcessosDto>(entity);
    }

    /// <summary>
    /// Deletes a advProcessos
    /// </summary>
    [Authorize(advProcessosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.sintese
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advProcessos.advProcessos> ApplyFilters(IQueryable<Sapienza.Lexus.advProcessos.advProcessos> queryable, advProcessosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.sintese.Contains(input.Filter) || x.numero.Contains(input.Filter) || x.dataDistribuicao.Contains(input.Filter) || x.estado.Contains(input.Filter) || x.cidade.Contains(input.Filter) || x.valorHonorariosTipo.Contains(input.Filter) || x.observacoes.Contains(input.Filter) || x.dataSentenca.Contains(input.Filter) || x.dataEncerramento.Contains(input.Filter) || x.recursoDataSentenca.Contains(input.Filter) || x.alvaraPendenteDesde.Contains(input.Filter) || x.historicoNumeros.Contains(input.Filter) || x.recebeBanco.Contains(input.Filter) || x.recebeDataLiberacao.Contains(input.Filter) || x.pendOutrosValoresDataEncerramento.Contains(input.Filter) || x.nomeResponsavel.Contains(input.Filter) || x.cpfResponsavel.Contains(input.Filter) || x.RPV.Contains(input.Filter) || x.bancarioBanco.Contains(input.Filter) || x.bancarioTipoConta.Contains(input.Filter) || x.bancarioAgencia.Contains(input.Filter) || x.bancarioConta.Contains(input.Filter) || x.bancarioFavorecido.Contains(input.Filter) || x.bancarioCpf.Contains(input.Filter) || x.nomeReu.Contains(input.Filter) || x.dataLiberacaoValorDeferido.Contains(input.Filter) || x.precatorio.Contains(input.Filter) || x.emitir.Contains(input.Filter) || x.formaRecebimento.Contains(input.Filter) || x.dataPrevisaoRepasseCliente.Contains(input.Filter) || x.honorariosTextoFicha.Contains(input.Filter) || x.valorHonorariosDestaqueTipo.Contains(input.Filter) || x.dataPrevisaoHonorariosDestaque.Contains(input.Filter) || x.dataPrevistaClienteReceber.Contains(input.Filter) || x.sucumbenciaAddData.Contains(input.Filter) || x.herdeirosTipoValor.Contains(input.Filter) || x.preProcessoPasta.Contains(input.Filter) || x.preProcessoDataCriacao.Contains(input.Filter) || x.preProcessoDataPrevista.Contains(input.Filter) || x.preProcessoDataRealizada.Contains(input.Filter) || x.tarifaParcelas.Contains(input.Filter) || x.dataEntrada.Contains(input.Filter))
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.idUsuarioInclusao != null, x => x.idUsuarioInclusao == input.idUsuarioInclusao)
            .WhereIf(input.idEscritorioOrigem != null, x => x.idEscritorioOrigem == input.idEscritorioOrigem)
            .WhereIf(input.idEscritorioResponsavel != null, x => x.idEscritorioResponsavel == input.idEscritorioResponsavel)
            .WhereIf(input.idAutorPeticao != null, x => x.idAutorPeticao == input.idAutorPeticao)
            .WhereIf(input.idResponsavel != null, x => x.idResponsavel == input.idResponsavel)
            .WhereIf(!input.sintese.IsNullOrWhiteSpace(), x => x.sintese.Contains(input.sintese))
            .WhereIf(!input.numero.IsNullOrWhiteSpace(), x => x.numero.Contains(input.numero))
            .WhereIf(!input.dataDistribuicao.IsNullOrWhiteSpace(), x => x.dataDistribuicao.Contains(input.dataDistribuicao))
            .WhereIf(input.idStatus != null, x => x.idStatus == input.idStatus)
            .WhereIf(input.idNatureza != null, x => x.idNatureza == input.idNatureza)
            .WhereIf(input.idTipo != null, x => x.idTipo == input.idTipo)
            .WhereIf(!input.estado.IsNullOrWhiteSpace(), x => x.estado.Contains(input.estado))
            .WhereIf(!input.cidade.IsNullOrWhiteSpace(), x => x.cidade.Contains(input.cidade))
            .WhereIf(input.idFase != null, x => x.idFase == input.idFase)
            .WhereIf(input.idRelevancia != null, x => x.idRelevancia == input.idRelevancia)
            .WhereIf(input.idProbabilidade != null, x => x.idProbabilidade == input.idProbabilidade)
            .WhereIf(input.valorCausa != null, x => x.valorCausa == input.valorCausa)
            .WhereIf(input.valorHonorarios != null, x => x.valorHonorarios == input.valorHonorarios)
            .WhereIf(!input.valorHonorariosTipo.IsNullOrWhiteSpace(), x => x.valorHonorariosTipo.Contains(input.valorHonorariosTipo))
            .WhereIf(!input.observacoes.IsNullOrWhiteSpace(), x => x.observacoes.Contains(input.observacoes))
            .WhereIf(input.idSentenca != null, x => x.idSentenca == input.idSentenca)
            .WhereIf(!input.dataSentenca.IsNullOrWhiteSpace(), x => x.dataSentenca.Contains(input.dataSentenca))
            .WhereIf(input.alvara != null, x => x.alvara == input.alvara)
            .WhereIf(input.valorDeferido != null, x => x.valorDeferido == input.valorDeferido)
            .WhereIf(!input.dataEncerramento.IsNullOrWhiteSpace(), x => x.dataEncerramento.Contains(input.dataEncerramento))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idOrgao != null, x => x.idOrgao == input.idOrgao)
            .WhereIf(input.idInstancia != null, x => x.idInstancia == input.idInstancia)
            .WhereIf(input.idVara != null, x => x.idVara == input.idVara)
            .WhereIf(input.recurso != null, x => x.recurso == input.recurso)
            .WhereIf(input.recursoIdSentenca != null, x => x.recursoIdSentenca == input.recursoIdSentenca)
            .WhereIf(!input.recursoDataSentenca.IsNullOrWhiteSpace(), x => x.recursoDataSentenca.Contains(input.recursoDataSentenca))
            .WhereIf(input.alvaraPendente != null, x => x.alvaraPendente == input.alvaraPendente)
            .WhereIf(!input.alvaraPendenteDesde.IsNullOrWhiteSpace(), x => x.alvaraPendenteDesde.Contains(input.alvaraPendenteDesde))
            .WhereIf(!input.historicoNumeros.IsNullOrWhiteSpace(), x => x.historicoNumeros.Contains(input.historicoNumeros))
            .WhereIf(input.recebeAcordo != null, x => x.recebeAcordo == input.recebeAcordo)
            .WhereIf(input.recebeRPV != null, x => x.recebeRPV == input.recebeRPV)
            .WhereIf(input.recebePrecatorio != null, x => x.recebePrecatorio == input.recebePrecatorio)
            .WhereIf(input.recebeAlvara != null, x => x.recebeAlvara == input.recebeAlvara)
            .WhereIf(!input.recebeBanco.IsNullOrWhiteSpace(), x => x.recebeBanco.Contains(input.recebeBanco))
            .WhereIf(!input.recebeDataLiberacao.IsNullOrWhiteSpace(), x => x.recebeDataLiberacao.Contains(input.recebeDataLiberacao))
            .WhereIf(input.pendOutrosValores != null, x => x.pendOutrosValores == input.pendOutrosValores)
            .WhereIf(!input.pendOutrosValoresDataEncerramento.IsNullOrWhiteSpace(), x => x.pendOutrosValoresDataEncerramento.Contains(input.pendOutrosValoresDataEncerramento))
            .WhereIf(input.pendOutrosValoresDeferido != null, x => x.pendOutrosValoresDeferido == input.pendOutrosValoresDeferido)
            .WhereIf(input.pendOutrosValoresValorDeferido != null, x => x.pendOutrosValoresValorDeferido == input.pendOutrosValoresValorDeferido)
            .WhereIf(input.acaoColetiva != null, x => x.acaoColetiva == input.acaoColetiva)
            .WhereIf(input.temResponsavel != null, x => x.temResponsavel == input.temResponsavel)
            .WhereIf(!input.nomeResponsavel.IsNullOrWhiteSpace(), x => x.nomeResponsavel.Contains(input.nomeResponsavel))
            .WhereIf(!input.cpfResponsavel.IsNullOrWhiteSpace(), x => x.cpfResponsavel.Contains(input.cpfResponsavel))
            .WhereIf(input.imposto != null, x => x.imposto == input.imposto)
            .WhereIf(input.tarifa != null, x => x.tarifa == input.tarifa)
            .WhereIf(input.complementoPositivo != null, x => x.complementoPositivo == input.complementoPositivo)
            .WhereIf(!input.RPV.IsNullOrWhiteSpace(), x => x.RPV.Contains(input.RPV))
            .WhereIf(!input.bancarioBanco.IsNullOrWhiteSpace(), x => x.bancarioBanco.Contains(input.bancarioBanco))
            .WhereIf(!input.bancarioTipoConta.IsNullOrWhiteSpace(), x => x.bancarioTipoConta.Contains(input.bancarioTipoConta))
            .WhereIf(!input.bancarioAgencia.IsNullOrWhiteSpace(), x => x.bancarioAgencia.Contains(input.bancarioAgencia))
            .WhereIf(!input.bancarioConta.IsNullOrWhiteSpace(), x => x.bancarioConta.Contains(input.bancarioConta))
            .WhereIf(!input.bancarioFavorecido.IsNullOrWhiteSpace(), x => x.bancarioFavorecido.Contains(input.bancarioFavorecido))
            .WhereIf(!input.bancarioCpf.IsNullOrWhiteSpace(), x => x.bancarioCpf.Contains(input.bancarioCpf))
            .WhereIf(!input.nomeReu.IsNullOrWhiteSpace(), x => x.nomeReu.Contains(input.nomeReu))
            .WhereIf(input.sucumbencia != null, x => x.sucumbencia == input.sucumbencia)
            .WhereIf(input.idConta != null, x => x.idConta == input.idConta)
            .WhereIf(!input.dataLiberacaoValorDeferido.IsNullOrWhiteSpace(), x => x.dataLiberacaoValorDeferido.Contains(input.dataLiberacaoValorDeferido))
            .WhereIf(input.boleto != null, x => x.boleto == input.boleto)
            .WhereIf(!input.precatorio.IsNullOrWhiteSpace(), x => x.precatorio.Contains(input.precatorio))
            .WhereIf(!input.emitir.IsNullOrWhiteSpace(), x => x.emitir.Contains(input.emitir))
            .WhereIf(input.emitido != null, x => x.emitido == input.emitido)
            .WhereIf(!input.formaRecebimento.IsNullOrWhiteSpace(), x => x.formaRecebimento.Contains(input.formaRecebimento))
            .WhereIf(input.bancarioBancoId != null, x => x.bancarioBancoId == input.bancarioBancoId)
            .WhereIf(!input.dataPrevisaoRepasseCliente.IsNullOrWhiteSpace(), x => x.dataPrevisaoRepasseCliente.Contains(input.dataPrevisaoRepasseCliente))
            .WhereIf(!input.honorariosTextoFicha.IsNullOrWhiteSpace(), x => x.honorariosTextoFicha.Contains(input.honorariosTextoFicha))
            .WhereIf(input.nfComComplementoPositivo != null, x => x.nfComComplementoPositivo == input.nfComComplementoPositivo)
            .WhereIf(input.valorHonorariosDestaque != null, x => x.valorHonorariosDestaque == input.valorHonorariosDestaque)
            .WhereIf(!input.valorHonorariosDestaqueTipo.IsNullOrWhiteSpace(), x => x.valorHonorariosDestaqueTipo.Contains(input.valorHonorariosDestaqueTipo))
            .WhereIf(!input.dataPrevisaoHonorariosDestaque.IsNullOrWhiteSpace(), x => x.dataPrevisaoHonorariosDestaque.Contains(input.dataPrevisaoHonorariosDestaque))
            .WhereIf(input.idContaPagar != null, x => x.idContaPagar == input.idContaPagar)
            .WhereIf(input.bancarioPerc != null, x => x.bancarioPerc == input.bancarioPerc)
            .WhereIf(!input.dataPrevistaClienteReceber.IsNullOrWhiteSpace(), x => x.dataPrevistaClienteReceber.Contains(input.dataPrevistaClienteReceber))
            .WhereIf(input.sucumbenciaAdd != null, x => x.sucumbenciaAdd == input.sucumbenciaAdd)
            .WhereIf(!input.sucumbenciaAddData.IsNullOrWhiteSpace(), x => x.sucumbenciaAddData.Contains(input.sucumbenciaAddData))
            .WhereIf(input.sucumbenciaAddIdBanco != null, x => x.sucumbenciaAddIdBanco == input.sucumbenciaAddIdBanco)
            .WhereIf(input.saldoDevedor != null, x => x.saldoDevedor == input.saldoDevedor)
            .WhereIf(!input.herdeirosTipoValor.IsNullOrWhiteSpace(), x => x.herdeirosTipoValor.Contains(input.herdeirosTipoValor))
            .WhereIf(input.nrParcelasProcesso != null, x => x.nrParcelasProcesso == input.nrParcelasProcesso)
            .WhereIf(input.nrParcelasSomenteSucumbencia != null, x => x.nrParcelasSomenteSucumbencia == input.nrParcelasSomenteSucumbencia)
            .WhereIf(input.preProcesso != null, x => x.preProcesso == input.preProcesso)
            .WhereIf(!input.preProcessoPasta.IsNullOrWhiteSpace(), x => x.preProcessoPasta.Contains(input.preProcessoPasta))
            .WhereIf(!input.preProcessoDataCriacao.IsNullOrWhiteSpace(), x => x.preProcessoDataCriacao.Contains(input.preProcessoDataCriacao))
            .WhereIf(!input.preProcessoDataPrevista.IsNullOrWhiteSpace(), x => x.preProcessoDataPrevista.Contains(input.preProcessoDataPrevista))
            .WhereIf(!input.preProcessoDataRealizada.IsNullOrWhiteSpace(), x => x.preProcessoDataRealizada.Contains(input.preProcessoDataRealizada))
            .WhereIf(input.preProcessoIdStatus != null, x => x.preProcessoIdStatus == input.preProcessoIdStatus)
            .WhereIf(input.tsConversao != null, x => x.tsConversao == input.tsConversao)
            .WhereIf(input.perdido != null, x => x.perdido == input.perdido)
            .WhereIf(input.tsPerdido != null, x => x.tsPerdido == input.tsPerdido)
            .WhereIf(input.idMotivoPerda != null, x => x.idMotivoPerda == input.idMotivoPerda)
            .WhereIf(input.convertido != null, x => x.convertido == input.convertido)
            .WhereIf(input.clientePrimeiraVez != null, x => x.clientePrimeiraVez == input.clientePrimeiraVez)
            .WhereIf(input.preProcessoIdTipo != null, x => x.preProcessoIdTipo == input.preProcessoIdTipo)
            .WhereIf(!input.tarifaParcelas.IsNullOrWhiteSpace(), x => x.tarifaParcelas.Contains(input.tarifaParcelas))
            .WhereIf(input.idOrigem != null, x => x.idOrigem == input.idOrigem)
            .WhereIf(!input.dataEntrada.IsNullOrWhiteSpace(), x => x.dataEntrada.Contains(input.dataEntrada))
            // ========== FK Filters ==========
            ;
    }
}
