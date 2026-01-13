using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabRegioes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabRegioes;

/// <summary>
/// Application service for fabRegioes entity
/// </summary>
[Authorize(fabRegioesPermissions.Default)]
public class fabRegioesAppService :
    LexusAppService,
    IfabRegioesAppService
{
    private readonly IRepository<Sapienza.Lexus.fabRegioes.fabRegioes, Guid> _repository;

    public fabRegioesAppService(
        IRepository<Sapienza.Lexus.fabRegioes.fabRegioes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fabRegioes by Id
    /// </summary>
    public virtual async Task<fabRegioesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabRegioes.fabRegioes, fabRegioesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabRegioeses
    /// </summary>
    public virtual async Task<PagedResultDto<fabRegioesDto>> GetListAsync(fabRegioesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabRegioes.fabRegioes>, List<fabRegioesDto>>(entities);

        return new PagedResultDto<fabRegioesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabRegioes
    /// </summary>
    [Authorize(fabRegioesPermissions.Create)]
    public virtual async Task<fabRegioesDto> CreateAsync(CreateUpdatefabRegioesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabRegioesDto, Sapienza.Lexus.fabRegioes.fabRegioes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabRegioes.fabRegioes, fabRegioesDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabRegioes
    /// </summary>
    [Authorize(fabRegioesPermissions.Update)]
    public virtual async Task<fabRegioesDto> UpdateAsync(Guid id, CreateUpdatefabRegioesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabRegioes.fabRegioes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabRegioes.fabRegioes, fabRegioesDto>(entity);
    }

    /// <summary>
    /// Deletes a fabRegioes
    /// </summary>
    [Authorize(fabRegioesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabRegioesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.fabRegioes.fabRegioes> ApplyFilters(IQueryable<Sapienza.Lexus.fabRegioes.fabRegioes> queryable, fabRegioesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.estados.Contains(input.Filter))
            .WhereIf(input.idRegiao != null, x => x.idRegiao == input.idRegiao)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(!input.estados.IsNullOrWhiteSpace(), x => x.estados.Contains(input.estados))
            // ========== FK Filters ==========
            ;
    }
}
