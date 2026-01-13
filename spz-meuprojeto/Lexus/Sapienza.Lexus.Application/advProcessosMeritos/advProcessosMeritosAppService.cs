using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProcessosMeritos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProcessosMeritos;

/// <summary>
/// Application service for advProcessosMeritos entity
/// </summary>
[Authorize(advProcessosMeritosPermissions.Default)]
public class advProcessosMeritosAppService :
    LexusAppService,
    IadvProcessosMeritosAppService
{
    private readonly IRepository<Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos, Guid> _repository;

    public advProcessosMeritosAppService(
        IRepository<Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProcessosMeritos by Id
    /// </summary>
    public virtual async Task<advProcessosMeritosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos, advProcessosMeritosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProcessosMeritoses
    /// </summary>
    public virtual async Task<PagedResultDto<advProcessosMeritosDto>> GetListAsync(advProcessosMeritosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos>, List<advProcessosMeritosDto>>(entities);

        return new PagedResultDto<advProcessosMeritosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProcessosMeritos
    /// </summary>
    [Authorize(advProcessosMeritosPermissions.Create)]
    public virtual async Task<advProcessosMeritosDto> CreateAsync(CreateUpdateadvProcessosMeritosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProcessosMeritosDto, Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos, advProcessosMeritosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProcessosMeritos
    /// </summary>
    [Authorize(advProcessosMeritosPermissions.Update)]
    public virtual async Task<advProcessosMeritosDto> UpdateAsync(Guid id, CreateUpdateadvProcessosMeritosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos, advProcessosMeritosDto>(entity);
    }

    /// <summary>
    /// Deletes a advProcessosMeritos
    /// </summary>
    [Authorize(advProcessosMeritosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosMeritosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Id.ToString()
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos> ApplyFilters(IQueryable<Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos> queryable, advProcessosMeritosGetListInput input)
    {
        return queryable
            .WhereIf(input.idProcessoMerito != null, x => x.idProcessoMerito == input.idProcessoMerito)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.idMerito != null, x => x.idMerito == input.idMerito)
            // ========== FK Filters ==========
            ;
    }
}
