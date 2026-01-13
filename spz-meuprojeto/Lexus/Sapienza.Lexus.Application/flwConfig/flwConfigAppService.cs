using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.flwConfig.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.flwConfig;

/// <summary>
/// Application service for flwConfig entity
/// </summary>
[Authorize(flwConfigPermissions.Default)]
public class flwConfigAppService :
    LexusAppService,
    IflwConfigAppService
{
    private readonly IRepository<Sapienza.Lexus.flwConfig.flwConfig, Guid> _repository;

    public flwConfigAppService(
        IRepository<Sapienza.Lexus.flwConfig.flwConfig, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single flwConfig by Id
    /// </summary>
    public virtual async Task<flwConfigDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.flwConfig.flwConfig, flwConfigDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of flwConfigs
    /// </summary>
    public virtual async Task<PagedResultDto<flwConfigDto>> GetListAsync(flwConfigGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.flwConfig.flwConfig>, List<flwConfigDto>>(entities);

        return new PagedResultDto<flwConfigDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new flwConfig
    /// </summary>
    [Authorize(flwConfigPermissions.Create)]
    public virtual async Task<flwConfigDto> CreateAsync(CreateUpdateflwConfigDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateflwConfigDto, Sapienza.Lexus.flwConfig.flwConfig>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.flwConfig.flwConfig, flwConfigDto>(entity);
    }

    /// <summary>
    /// Updates an existing flwConfig
    /// </summary>
    [Authorize(flwConfigPermissions.Update)]
    public virtual async Task<flwConfigDto> UpdateAsync(Guid id, CreateUpdateflwConfigDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.flwConfig.flwConfig), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.flwConfig.flwConfig, flwConfigDto>(entity);
    }

    /// <summary>
    /// Deletes a flwConfig
    /// </summary>
    [Authorize(flwConfigPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetflwConfigLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.tipoMarcacoes
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.flwConfig.flwConfig> ApplyFilters(IQueryable<Sapienza.Lexus.flwConfig.flwConfig> queryable, flwConfigGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.tipoMarcacoes.Contains(input.Filter))
            .WhereIf(input.idConfig != null, x => x.idConfig == input.idConfig)
            .WhereIf(!input.tipoMarcacoes.IsNullOrWhiteSpace(), x => x.tipoMarcacoes.Contains(input.tipoMarcacoes))
            // ========== FK Filters ==========
            ;
    }
}
