using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.SchedulerType.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SchedulerType;

/// <summary>
/// Application service for SchedulerType entity
/// </summary>
[Authorize(SchedulerTypePermissions.Default)]
public class SchedulerTypeAppService :
    DominusAppService,
    ISchedulerTypeAppService
{
    private readonly IRepository<Dominus.SchedulerType.SchedulerType, Guid> _repository;

    public SchedulerTypeAppService(
        IRepository<Dominus.SchedulerType.SchedulerType, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single SchedulerType by Id
    /// </summary>
    public virtual async Task<SchedulerTypeDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.SchedulerType.SchedulerType, SchedulerTypeDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of SchedulerTypes
    /// </summary>
    public virtual async Task<PagedResultDto<SchedulerTypeDto>> GetListAsync(SchedulerTypeGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.SchedulerType.SchedulerType>, List<SchedulerTypeDto>>(entities);

        return new PagedResultDto<SchedulerTypeDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new SchedulerType
    /// </summary>
    [Authorize(SchedulerTypePermissions.Create)]
    public virtual async Task<SchedulerTypeDto> CreateAsync(CreateUpdateSchedulerTypeDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateSchedulerTypeDto, Dominus.SchedulerType.SchedulerType>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SchedulerType.SchedulerType, SchedulerTypeDto>(entity);
    }

    /// <summary>
    /// Updates an existing SchedulerType
    /// </summary>
    [Authorize(SchedulerTypePermissions.Update)]
    public virtual async Task<SchedulerTypeDto> UpdateAsync(Guid id, CreateUpdateSchedulerTypeDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.SchedulerType.SchedulerType), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SchedulerType.SchedulerType, SchedulerTypeDto>(entity);
    }

    /// <summary>
    /// Deletes a SchedulerType
    /// </summary>
    [Authorize(SchedulerTypePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSchedulerTypeLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Name
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.SchedulerType.SchedulerType> ApplyFilters(IQueryable<Dominus.SchedulerType.SchedulerType> queryable, SchedulerTypeGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
