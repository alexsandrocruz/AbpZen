using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finPlanoContas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finPlanoContas;

/// <summary>
/// Application service for finPlanoContas entity
/// </summary>
[Authorize(finPlanoContasPermissions.Default)]
public class finPlanoContasAppService :
    LexusAppService,
    IfinPlanoContasAppService
{
    private readonly IRepository<Sapienza.Lexus.finPlanoContas.finPlanoContas, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.finLancamentos.finLancamentos, Guid> _finLancamentosRepository;

    public finPlanoContasAppService(
        IRepository<Sapienza.Lexus.finPlanoContas.finPlanoContas, Guid> repository,
        IRepository<Sapienza.Lexus.finLancamentos.finLancamentos, Guid> finLancamentosRepository
    )
    {
        _repository = repository;
        _finLancamentosRepository = finLancamentosRepository;
    }

    /// <summary>
    /// Gets a single finPlanoContas by Id
    /// </summary>
    public virtual async Task<finPlanoContasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finPlanoContas.finPlanoContas, finPlanoContasDto>(entity);
        if (entity.finLancamentosId != null)
        {
            var parent = await _finLancamentosRepository.FindAsync(entity.finLancamentosId.Value);
            dto.finLancamentosDisplayName = parent?.operacao;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finPlanoContases
    /// </summary>
    public virtual async Task<PagedResultDto<finPlanoContasDto>> GetListAsync(finPlanoContasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finPlanoContas.finPlanoContas>, List<finPlanoContasDto>>(entities);
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

        return new PagedResultDto<finPlanoContasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finPlanoContas
    /// </summary>
    [Authorize(finPlanoContasPermissions.Create)]
    public virtual async Task<finPlanoContasDto> CreateAsync(CreateUpdatefinPlanoContasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinPlanoContasDto, Sapienza.Lexus.finPlanoContas.finPlanoContas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finPlanoContas.finPlanoContas, finPlanoContasDto>(entity);
    }

    /// <summary>
    /// Updates an existing finPlanoContas
    /// </summary>
    [Authorize(finPlanoContasPermissions.Update)]
    public virtual async Task<finPlanoContasDto> UpdateAsync(Guid id, CreateUpdatefinPlanoContasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finPlanoContas.finPlanoContas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finPlanoContas.finPlanoContas, finPlanoContasDto>(entity);
    }

    /// <summary>
    /// Deletes a finPlanoContas
    /// </summary>
    [Authorize(finPlanoContasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinPlanoContasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finPlanoContas.finPlanoContas> ApplyFilters(IQueryable<Sapienza.Lexus.finPlanoContas.finPlanoContas> queryable, finPlanoContasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.codigo.Contains(input.Filter))
            .WhereIf(input.idPlanoConta != null, x => x.idPlanoConta == input.idPlanoConta)
            .WhereIf(input.idGrupo != null, x => x.idGrupo == input.idGrupo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(!input.codigo.IsNullOrWhiteSpace(), x => x.codigo.Contains(input.codigo))
            .WhereIf(input.pagamentoSempreLiberado != null, x => x.pagamentoSempreLiberado == input.pagamentoSempreLiberado)
            .WhereIf(input.permiteLancamentoQuitado != null, x => x.permiteLancamentoQuitado == input.permiteLancamentoQuitado)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.padraoVendas != null, x => x.padraoVendas == input.padraoVendas)
            .WhereIf(input.antecipaVencimento != null, x => x.antecipaVencimento == input.antecipaVencimento)
            .WhereIf(input.terceiroNivel != null, x => x.terceiroNivel == input.terceiroNivel)
            .WhereIf(input.criarPeloFinanceiro != null, x => x.criarPeloFinanceiro == input.criarPeloFinanceiro)
            .WhereIf(input.valoresRestritos != null, x => x.valoresRestritos == input.valoresRestritos)
            .WhereIf(input.naoAbatePagtoDoSaldoDoCliente != null, x => x.naoAbatePagtoDoSaldoDoCliente == input.naoAbatePagtoDoSaldoDoCliente)
            // ========== FK Filters ==========
            .WhereIf(input.finLancamentosId != null, x => x.finLancamentosId == input.finLancamentosId)
            ;
    }
}
