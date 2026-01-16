using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabCondicoesPagamento.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabCondicoesPagamento;

/// <summary>
/// Application service for fabCondicoesPagamento entity
/// </summary>
[Authorize(fabCondicoesPagamentoPermissions.Default)]
public class fabCondicoesPagamentoAppService :
    LexusAppService,
    IfabCondicoesPagamentoAppService
{
    private readonly IRepository<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.opoOrcamentos.opoOrcamentos, Guid> _opoOrcamentosRepository;

    public fabCondicoesPagamentoAppService(
        IRepository<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento, Guid> repository,
        IRepository<Sapienza.Lexus.opoOrcamentos.opoOrcamentos, Guid> opoOrcamentosRepository
    )
    {
        _repository = repository;
        _opoOrcamentosRepository = opoOrcamentosRepository;
    }

    /// <summary>
    /// Gets a single fabCondicoesPagamento by Id
    /// </summary>
    public virtual async Task<fabCondicoesPagamentoDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento, fabCondicoesPagamentoDto>(entity);
        if (entity.opoOrcamentosId != null)
        {
            var parent = await _opoOrcamentosRepository.FindAsync(entity.opoOrcamentosId.Value);
            dto.opoOrcamentosDisplayName = parent?.titulo;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabCondicoesPagamentos
    /// </summary>
    public virtual async Task<PagedResultDto<fabCondicoesPagamentoDto>> GetListAsync(fabCondicoesPagamentoGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento>, List<fabCondicoesPagamentoDto>>(entities);
        var opoOrcamentosIds = entities
            .Where(x => x.opoOrcamentosId != null)
            .Select(x => x.opoOrcamentosId.Value)
            .Distinct()
            .ToList();

        if (opoOrcamentosIds.Any())
        {
            var parents = await _opoOrcamentosRepository.GetListAsync(x => opoOrcamentosIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.titulo);

            foreach (var dto in dtoList.Where(x => x.opoOrcamentosId != null))
            {
                if (parentMap.TryGetValue(dto.opoOrcamentosId.Value, out var displayName))
                {
                    dto.opoOrcamentosDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<fabCondicoesPagamentoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabCondicoesPagamento
    /// </summary>
    [Authorize(fabCondicoesPagamentoPermissions.Create)]
    public virtual async Task<fabCondicoesPagamentoDto> CreateAsync(CreateUpdatefabCondicoesPagamentoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabCondicoesPagamentoDto, Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento, fabCondicoesPagamentoDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabCondicoesPagamento
    /// </summary>
    [Authorize(fabCondicoesPagamentoPermissions.Update)]
    public virtual async Task<fabCondicoesPagamentoDto> UpdateAsync(Guid id, CreateUpdatefabCondicoesPagamentoDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento, fabCondicoesPagamentoDto>(entity);
    }

    /// <summary>
    /// Deletes a fabCondicoesPagamento
    /// </summary>
    [Authorize(fabCondicoesPagamentoPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabCondicoesPagamentoLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento> ApplyFilters(IQueryable<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento> queryable, fabCondicoesPagamentoGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idCondicaoPagamento != null, x => x.idCondicaoPagamento == input.idCondicaoPagamento)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.parcelas != null, x => x.parcelas == input.parcelas)
            .WhereIf(input.p1 != null, x => x.p1 == input.p1)
            .WhereIf(input.d1 != null, x => x.d1 == input.d1)
            .WhereIf(input.p2 != null, x => x.p2 == input.p2)
            .WhereIf(input.d2 != null, x => x.d2 == input.d2)
            .WhereIf(input.p3 != null, x => x.p3 == input.p3)
            .WhereIf(input.d3 != null, x => x.d3 == input.d3)
            .WhereIf(input.p4 != null, x => x.p4 == input.p4)
            .WhereIf(input.d4 != null, x => x.d4 == input.d4)
            .WhereIf(input.p5 != null, x => x.p5 == input.p5)
            .WhereIf(input.d5 != null, x => x.d5 == input.d5)
            .WhereIf(input.p6 != null, x => x.p6 == input.p6)
            .WhereIf(input.d6 != null, x => x.d6 == input.d6)
            .WhereIf(input.p7 != null, x => x.p7 == input.p7)
            .WhereIf(input.d7 != null, x => x.d7 == input.d7)
            .WhereIf(input.p8 != null, x => x.p8 == input.p8)
            .WhereIf(input.d8 != null, x => x.d8 == input.d8)
            .WhereIf(input.p9 != null, x => x.p9 == input.p9)
            .WhereIf(input.d9 != null, x => x.d9 == input.d9)
            .WhereIf(input.p10 != null, x => x.p10 == input.p10)
            .WhereIf(input.d10 != null, x => x.d10 == input.d10)
            .WhereIf(input.p11 != null, x => x.p11 == input.p11)
            .WhereIf(input.d11 != null, x => x.d11 == input.d11)
            .WhereIf(input.p12 != null, x => x.p12 == input.p12)
            .WhereIf(input.d12 != null, x => x.d12 == input.d12)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.compras != null, x => x.compras == input.compras)
            .WhereIf(input.vendas != null, x => x.vendas == input.vendas)
            .WhereIf(input.valorMinimo != null, x => x.valorMinimo == input.valorMinimo)
            .WhereIf(input.atendimento != null, x => x.atendimento == input.atendimento)
            // ========== FK Filters ==========
            .WhereIf(input.opoOrcamentosId != null, x => x.opoOrcamentosId == input.opoOrcamentosId)
            ;
    }
}
