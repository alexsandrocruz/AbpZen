using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabFormasPagamento.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabFormasPagamento;

/// <summary>
/// Application service for fabFormasPagamento entity
/// </summary>
[Authorize(fabFormasPagamentoPermissions.Default)]
public class fabFormasPagamentoAppService :
    LexusAppService,
    IfabFormasPagamentoAppService
{
    private readonly IRepository<Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento, Guid> _repository;

    public fabFormasPagamentoAppService(
        IRepository<Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fabFormasPagamento by Id
    /// </summary>
    public virtual async Task<fabFormasPagamentoDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento, fabFormasPagamentoDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabFormasPagamentos
    /// </summary>
    public virtual async Task<PagedResultDto<fabFormasPagamentoDto>> GetListAsync(fabFormasPagamentoGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento>, List<fabFormasPagamentoDto>>(entities);

        return new PagedResultDto<fabFormasPagamentoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabFormasPagamento
    /// </summary>
    [Authorize(fabFormasPagamentoPermissions.Create)]
    public virtual async Task<fabFormasPagamentoDto> CreateAsync(CreateUpdatefabFormasPagamentoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabFormasPagamentoDto, Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento, fabFormasPagamentoDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabFormasPagamento
    /// </summary>
    [Authorize(fabFormasPagamentoPermissions.Update)]
    public virtual async Task<fabFormasPagamentoDto> UpdateAsync(Guid id, CreateUpdatefabFormasPagamentoDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento, fabFormasPagamentoDto>(entity);
    }

    /// <summary>
    /// Deletes a fabFormasPagamento
    /// </summary>
    [Authorize(fabFormasPagamentoPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabFormasPagamentoLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento> ApplyFilters(IQueryable<Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento> queryable, fabFormasPagamentoGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idFormaPagamento != null, x => x.idFormaPagamento == input.idFormaPagamento)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ordem != null, x => x.ordem == input.ordem)
            .WhereIf(input.padrao != null, x => x.padrao == input.padrao)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idCondicaoPagamento != null, x => x.idCondicaoPagamento == input.idCondicaoPagamento)
            .WhereIf(input.contasPagar != null, x => x.contasPagar == input.contasPagar)
            .WhereIf(input.compras != null, x => x.compras == input.compras)
            // ========== FK Filters ==========
            ;
    }
}
