using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCliSituacoes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliSituacoes;

/// <summary>
/// Application service for advCliSituacoes entity
/// </summary>
[Authorize(advCliSituacoesPermissions.Default)]
public class advCliSituacoesAppService :
    LexusAppService,
    IadvCliSituacoesAppService
{
    private readonly IRepository<Sapienza.Lexus.advCliSituacoes.advCliSituacoes, Guid> _repository;

    public advCliSituacoesAppService(
        IRepository<Sapienza.Lexus.advCliSituacoes.advCliSituacoes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advCliSituacoes by Id
    /// </summary>
    public virtual async Task<advCliSituacoesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCliSituacoes.advCliSituacoes, advCliSituacoesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCliSituacoeses
    /// </summary>
    public virtual async Task<PagedResultDto<advCliSituacoesDto>> GetListAsync(advCliSituacoesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCliSituacoes.advCliSituacoes>, List<advCliSituacoesDto>>(entities);

        return new PagedResultDto<advCliSituacoesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCliSituacoes
    /// </summary>
    [Authorize(advCliSituacoesPermissions.Create)]
    public virtual async Task<advCliSituacoesDto> CreateAsync(CreateUpdateadvCliSituacoesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCliSituacoesDto, Sapienza.Lexus.advCliSituacoes.advCliSituacoes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliSituacoes.advCliSituacoes, advCliSituacoesDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCliSituacoes
    /// </summary>
    [Authorize(advCliSituacoesPermissions.Update)]
    public virtual async Task<advCliSituacoesDto> UpdateAsync(Guid id, CreateUpdateadvCliSituacoesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCliSituacoes.advCliSituacoes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliSituacoes.advCliSituacoes, advCliSituacoesDto>(entity);
    }

    /// <summary>
    /// Deletes a advCliSituacoes
    /// </summary>
    [Authorize(advCliSituacoesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCliSituacoesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advCliSituacoes.advCliSituacoes> ApplyFilters(IQueryable<Sapienza.Lexus.advCliSituacoes.advCliSituacoes> queryable, advCliSituacoesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idSituacao != null, x => x.idSituacao == input.idSituacao)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
