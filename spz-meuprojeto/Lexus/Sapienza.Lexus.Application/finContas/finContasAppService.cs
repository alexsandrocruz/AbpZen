using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finContas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finContas;

/// <summary>
/// Application service for finContas entity
/// </summary>
[Authorize(finContasPermissions.Default)]
public class finContasAppService :
    LexusAppService,
    IfinContasAppService
{
    private readonly IRepository<Sapienza.Lexus.finContas.finContas, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.finExtrato.finExtrato, Guid> _finExtratoRepository;
    private readonly IRepository<Sapienza.Lexus.finLancamentos.finLancamentos, Guid> _finLancamentosRepository;

    public finContasAppService(
        IRepository<Sapienza.Lexus.finContas.finContas, Guid> repository,
        IRepository<Sapienza.Lexus.finExtrato.finExtrato, Guid> finExtratoRepository,
        IRepository<Sapienza.Lexus.finLancamentos.finLancamentos, Guid> finLancamentosRepository
    )
    {
        _repository = repository;
        _finExtratoRepository = finExtratoRepository;
        _finLancamentosRepository = finLancamentosRepository;
    }

    /// <summary>
    /// Gets a single finContas by Id
    /// </summary>
    public virtual async Task<finContasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finContas.finContas, finContasDto>(entity);
        if (entity.finExtratoId != null)
        {
            var parent = await _finExtratoRepository.FindAsync(entity.finExtratoId.Value);
            dto.finExtratoDisplayName = parent?.data;
        }
        if (entity.finLancamentosId != null)
        {
            var parent = await _finLancamentosRepository.FindAsync(entity.finLancamentosId.Value);
            dto.finLancamentosDisplayName = parent?.operacao;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finContases
    /// </summary>
    public virtual async Task<PagedResultDto<finContasDto>> GetListAsync(finContasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finContas.finContas>, List<finContasDto>>(entities);
        var finExtratoIds = entities
            .Where(x => x.finExtratoId != null)
            .Select(x => x.finExtratoId.Value)
            .Distinct()
            .ToList();

        if (finExtratoIds.Any())
        {
            var parents = await _finExtratoRepository.GetListAsync(x => finExtratoIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.data);

            foreach (var dto in dtoList.Where(x => x.finExtratoId != null))
            {
                if (parentMap.TryGetValue(dto.finExtratoId.Value, out var displayName))
                {
                    dto.finExtratoDisplayName = displayName;
                }
            }
        }
        var finLancamentosIds = entities
            .Where(x => x.finLancamentosId != null)
            .Select(x => x.finLancamentosId.Value)
            .Distinct()
            .ToList();

        if (finLancamentosIds.Any())
        {
            var parents = await _finLancamentosRepository.GetListAsync(x => finLancamentosIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.operacao);

            foreach (var dto in dtoList.Where(x => x.finLancamentosId != null))
            {
                if (parentMap.TryGetValue(dto.finLancamentosId.Value, out var displayName))
                {
                    dto.finLancamentosDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<finContasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finContas
    /// </summary>
    [Authorize(finContasPermissions.Create)]
    public virtual async Task<finContasDto> CreateAsync(CreateUpdatefinContasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinContasDto, Sapienza.Lexus.finContas.finContas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finContas.finContas, finContasDto>(entity);
    }

    /// <summary>
    /// Updates an existing finContas
    /// </summary>
    [Authorize(finContasPermissions.Update)]
    public virtual async Task<finContasDto> UpdateAsync(Guid id, CreateUpdatefinContasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finContas.finContas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finContas.finContas, finContasDto>(entity);
    }

    /// <summary>
    /// Deletes a finContas
    /// </summary>
    [Authorize(finContasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinContasLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.titulo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.finContas.finContas> ApplyFilters(IQueryable<Sapienza.Lexus.finContas.finContas> queryable, finContasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.banco.Contains(input.Filter) || x.agencia.Contains(input.Filter) || x.conta.Contains(input.Filter) || x.favorecido.Contains(input.Filter) || x.codigo.Contains(input.Filter) || x.cor.Contains(input.Filter))
            .WhereIf(input.idConta != null, x => x.idConta == input.idConta)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(!input.banco.IsNullOrWhiteSpace(), x => x.banco.Contains(input.banco))
            .WhereIf(!input.agencia.IsNullOrWhiteSpace(), x => x.agencia.Contains(input.agencia))
            .WhereIf(!input.conta.IsNullOrWhiteSpace(), x => x.conta.Contains(input.conta))
            .WhereIf(!input.favorecido.IsNullOrWhiteSpace(), x => x.favorecido.Contains(input.favorecido))
            .WhereIf(input.limite != null, x => x.limite == input.limite)
            .WhereIf(input.padraoFluxo != null, x => x.padraoFluxo == input.padraoFluxo)
            .WhereIf(input.considerarIndicador != null, x => x.considerarIndicador == input.considerarIndicador)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.saldoInicial != null, x => x.saldoInicial == input.saldoInicial)
            .WhereIf(input.padrao != null, x => x.padrao == input.padrao)
            .WhereIf(!input.codigo.IsNullOrWhiteSpace(), x => x.codigo.Contains(input.codigo))
            .WhereIf(!input.cor.IsNullOrWhiteSpace(), x => x.cor.Contains(input.cor))
            // ========== FK Filters ==========
            .WhereIf(input.finExtratoId != null, x => x.finExtratoId == input.finExtratoId)
            .WhereIf(input.finLancamentosId != null, x => x.finLancamentosId == input.finLancamentosId)
            ;
    }
}
