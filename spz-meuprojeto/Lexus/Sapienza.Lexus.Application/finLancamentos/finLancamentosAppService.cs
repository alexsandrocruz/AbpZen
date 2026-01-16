using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finLancamentos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finLancamentos;

/// <summary>
/// Application service for finLancamentos entity
/// </summary>
[Authorize(finLancamentosPermissions.Default)]
public class finLancamentosAppService :
    LexusAppService,
    IfinLancamentosAppService
{
    private readonly IRepository<Sapienza.Lexus.finLancamentos.finLancamentos, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas, Guid> _finPrestacaoContasRepository;

    public finLancamentosAppService(
        IRepository<Sapienza.Lexus.finLancamentos.finLancamentos, Guid> repository,
        IRepository<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas, Guid> finPrestacaoContasRepository
    )
    {
        _repository = repository;
        _finPrestacaoContasRepository = finPrestacaoContasRepository;
    }

    /// <summary>
    /// Gets a single finLancamentos by Id
    /// </summary>
    public virtual async Task<finLancamentosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finLancamentos.finLancamentos, finLancamentosDto>(entity);
        var finPrestacaoContas = await _finPrestacaoContasRepository.FindAsync(entity.finPrestacaoContasId);
        dto.finPrestacaoContasDisplayName = finPrestacaoContas?.Id.ToString();

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finLancamentoses
    /// </summary>
    public virtual async Task<PagedResultDto<finLancamentosDto>> GetListAsync(finLancamentosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finLancamentos.finLancamentos>, List<finLancamentosDto>>(entities);
        var finPrestacaoContasIds = entities
            .Select(x => x.finPrestacaoContasId)
            .Distinct()
            .ToList();

        if (finPrestacaoContasIds.Any())
        {
            var parents = await _finPrestacaoContasRepository.GetListAsync(x => finPrestacaoContasIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Id.ToString());

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.finPrestacaoContasId, out var displayName))
                {
                    dto.finPrestacaoContasDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<finLancamentosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finLancamentos
    /// </summary>
    [Authorize(finLancamentosPermissions.Create)]
    public virtual async Task<finLancamentosDto> CreateAsync(CreateUpdatefinLancamentosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinLancamentosDto, Sapienza.Lexus.finLancamentos.finLancamentos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finLancamentos.finLancamentos, finLancamentosDto>(entity);
    }

    /// <summary>
    /// Updates an existing finLancamentos
    /// </summary>
    [Authorize(finLancamentosPermissions.Update)]
    public virtual async Task<finLancamentosDto> UpdateAsync(Guid id, CreateUpdatefinLancamentosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finLancamentos.finLancamentos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finLancamentos.finLancamentos, finLancamentosDto>(entity);
    }

    /// <summary>
    /// Deletes a finLancamentos
    /// </summary>
    [Authorize(finLancamentosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinLancamentosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.operacao
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.finLancamentos.finLancamentos> ApplyFilters(IQueryable<Sapienza.Lexus.finLancamentos.finLancamentos> queryable, finLancamentosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.operacao.Contains(input.Filter) || x.modulo.Contains(input.Filter) || x.descricao.Contains(input.Filter) || x.nrDocumento.Contains(input.Filter) || x.dataEmissao.Contains(input.Filter) || x.dataVencimento.Contains(input.Filter) || x.dataQuitacao.Contains(input.Filter) || x.recorrenteChave.Contains(input.Filter) || x.observacao.Contains(input.Filter) || x.dataVencimentoOriginal.Contains(input.Filter) || x.dataParaPrevisao.Contains(input.Filter) || x.arquivoDocumento.Contains(input.Filter) || x.arquivoComprovante.Contains(input.Filter) || x.identificacaoPagar.Contains(input.Filter) || x.identificacaoPagar2.Contains(input.Filter) || x.arquivoDocumento2.Contains(input.Filter) || x.arquivoComprovante2.Contains(input.Filter) || x.verbaDataDe.Contains(input.Filter) || x.verbaDataAte.Contains(input.Filter) || x.verbaEstado.Contains(input.Filter) || x.verbaCidade.Contains(input.Filter))
            .WhereIf(input.idLancamento != null, x => x.idLancamento == input.idLancamento)
            .WhereIf(input.idConta != null, x => x.idConta == input.idConta)
            .WhereIf(input.idPlanoConta != null, x => x.idPlanoConta == input.idPlanoConta)
            .WhereIf(input.idCentroCusto != null, x => x.idCentroCusto == input.idCentroCusto)
            .WhereIf(!input.operacao.IsNullOrWhiteSpace(), x => x.operacao.Contains(input.operacao))
            .WhereIf(input.idForma != null, x => x.idForma == input.idForma)
            .WhereIf(!input.modulo.IsNullOrWhiteSpace(), x => x.modulo.Contains(input.modulo))
            .WhereIf(input.idCadastro != null, x => x.idCadastro == input.idCadastro)
            .WhereIf(input.idPedido != null, x => x.idPedido == input.idPedido)
            .WhereIf(!input.descricao.IsNullOrWhiteSpace(), x => x.descricao.Contains(input.descricao))
            .WhereIf(!input.nrDocumento.IsNullOrWhiteSpace(), x => x.nrDocumento.Contains(input.nrDocumento))
            .WhereIf(input.valor != null, x => x.valor == input.valor)
            .WhereIf(!input.dataEmissao.IsNullOrWhiteSpace(), x => x.dataEmissao.Contains(input.dataEmissao))
            .WhereIf(!input.dataVencimento.IsNullOrWhiteSpace(), x => x.dataVencimento.Contains(input.dataVencimento))
            .WhereIf(!input.dataQuitacao.IsNullOrWhiteSpace(), x => x.dataQuitacao.Contains(input.dataQuitacao))
            .WhereIf(input.quitado != null, x => x.quitado == input.quitado)
            .WhereIf(input.recorrente != null, x => x.recorrente == input.recorrente)
            .WhereIf(!input.recorrenteChave.IsNullOrWhiteSpace(), x => x.recorrenteChave.Contains(input.recorrenteChave))
            .WhereIf(input.previsao != null, x => x.previsao == input.previsao)
            .WhereIf(input.cobrancaEnviada != null, x => x.cobrancaEnviada == input.cobrancaEnviada)
            .WhereIf(input.parcelado != null, x => x.parcelado == input.parcelado)
            .WhereIf(input.identificacao != null, x => x.identificacao == input.identificacao)
            .WhereIf(!input.observacao.IsNullOrWhiteSpace(), x => x.observacao.Contains(input.observacao))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idUsuarioInclusao != null, x => x.idUsuarioInclusao == input.idUsuarioInclusao)
            .WhereIf(input.idUsuarioAlteracao != null, x => x.idUsuarioAlteracao == input.idUsuarioAlteracao)
            .WhereIf(input.parcela != null, x => x.parcela == input.parcela)
            .WhereIf(input.parcelaMaxima != null, x => x.parcelaMaxima == input.parcelaMaxima)
            .WhereIf(!input.dataVencimentoOriginal.IsNullOrWhiteSpace(), x => x.dataVencimentoOriginal.Contains(input.dataVencimentoOriginal))
            .WhereIf(input.pagtoLiberado != null, x => x.pagtoLiberado == input.pagtoLiberado)
            .WhereIf(!input.dataParaPrevisao.IsNullOrWhiteSpace(), x => x.dataParaPrevisao.Contains(input.dataParaPrevisao))
            .WhereIf(input.recorrenteVencendoVisto != null, x => x.recorrenteVencendoVisto == input.recorrenteVencendoVisto)
            .WhereIf(input.recebimentoFuturo != null, x => x.recebimentoFuturo == input.recebimentoFuturo)
            .WhereIf(input.recebimentoFuturoRel != null, x => x.recebimentoFuturoRel == input.recebimentoFuturoRel)
            .WhereIf(input.idTerceiro != null, x => x.idTerceiro == input.idTerceiro)
            .WhereIf(!input.arquivoDocumento.IsNullOrWhiteSpace(), x => x.arquivoDocumento.Contains(input.arquivoDocumento))
            .WhereIf(!input.arquivoComprovante.IsNullOrWhiteSpace(), x => x.arquivoComprovante.Contains(input.arquivoComprovante))
            .WhereIf(input.idClientePagar != null, x => x.idClientePagar == input.idClientePagar)
            .WhereIf(input.idProcessoPagar != null, x => x.idProcessoPagar == input.idProcessoPagar)
            .WhereIf(input.idArea != null, x => x.idArea == input.idArea)
            .WhereIf(!input.identificacaoPagar.IsNullOrWhiteSpace(), x => x.identificacaoPagar.Contains(input.identificacaoPagar))
            .WhereIf(!input.identificacaoPagar2.IsNullOrWhiteSpace(), x => x.identificacaoPagar2.Contains(input.identificacaoPagar2))
            .WhereIf(!input.arquivoDocumento2.IsNullOrWhiteSpace(), x => x.arquivoDocumento2.Contains(input.arquivoDocumento2))
            .WhereIf(!input.arquivoComprovante2.IsNullOrWhiteSpace(), x => x.arquivoComprovante2.Contains(input.arquivoComprovante2))
            .WhereIf(input.verba != null, x => x.verba == input.verba)
            .WhereIf(!input.verbaDataDe.IsNullOrWhiteSpace(), x => x.verbaDataDe.Contains(input.verbaDataDe))
            .WhereIf(!input.verbaDataAte.IsNullOrWhiteSpace(), x => x.verbaDataAte.Contains(input.verbaDataAte))
            .WhereIf(!input.verbaEstado.IsNullOrWhiteSpace(), x => x.verbaEstado.Contains(input.verbaEstado))
            .WhereIf(!input.verbaCidade.IsNullOrWhiteSpace(), x => x.verbaCidade.Contains(input.verbaCidade))
            .WhereIf(input.idCentroResultado != null, x => x.idCentroResultado == input.idCentroResultado)
            .WhereIf(input.secundaria != null, x => x.secundaria == input.secundaria)
            .WhereIf(input.geradoPeloProcesso != null, x => x.geradoPeloProcesso == input.geradoPeloProcesso)
            .WhereIf(input.sequenciaHerdeiro != null, x => x.sequenciaHerdeiro == input.sequenciaHerdeiro)
            .WhereIf(input.idUnidade != null, x => x.idUnidade == input.idUnidade)
            .WhereIf(input.rateioFeito != null, x => x.rateioFeito == input.rateioFeito)
            .WhereIf(input.naoAbatePagtoDoSaldoDoCliente != null, x => x.naoAbatePagtoDoSaldoDoCliente == input.naoAbatePagtoDoSaldoDoCliente)
            .WhereIf(input.idHonorario != null, x => x.idHonorario == input.idHonorario)
            // ========== FK Filters ==========
            .WhereIf(input.finPrestacaoContasId != null, x => x.finPrestacaoContasId == input.finPrestacaoContasId)
            ;
    }
}
