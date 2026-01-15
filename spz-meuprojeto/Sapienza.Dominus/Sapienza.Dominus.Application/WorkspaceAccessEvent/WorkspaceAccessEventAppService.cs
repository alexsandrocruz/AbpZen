using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.WorkspaceAccessEvent.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WorkspaceAccessEvent;

/// <summary>
/// Application service for WorkspaceAccessEvent entity
/// </summary>
[Authorize(WorkspaceAccessEventPermissions.Default)]
public class WorkspaceAccessEventAppService :
    DominusAppService,
    IWorkspaceAccessEventAppService
{
    private readonly IRepository<Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent, Guid> _repository;

    public WorkspaceAccessEventAppService(
        IRepository<Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single WorkspaceAccessEvent by Id
    /// </summary>
    public virtual async Task<WorkspaceAccessEventDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent, WorkspaceAccessEventDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of WorkspaceAccessEvents
    /// </summary>
    public virtual async Task<PagedResultDto<WorkspaceAccessEventDto>> GetListAsync(WorkspaceAccessEventGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent>, List<WorkspaceAccessEventDto>>(entities);

        return new PagedResultDto<WorkspaceAccessEventDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new WorkspaceAccessEvent
    /// </summary>
    [Authorize(WorkspaceAccessEventPermissions.Create)]
    public virtual async Task<WorkspaceAccessEventDto> CreateAsync(CreateUpdateWorkspaceAccessEventDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateWorkspaceAccessEventDto, Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent, WorkspaceAccessEventDto>(entity);
    }

    /// <summary>
    /// Updates an existing WorkspaceAccessEvent
    /// </summary>
    [Authorize(WorkspaceAccessEventPermissions.Update)]
    public virtual async Task<WorkspaceAccessEventDto> UpdateAsync(Guid id, CreateUpdateWorkspaceAccessEventDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent, WorkspaceAccessEventDto>(entity);
    }

    /// <summary>
    /// Deletes a WorkspaceAccessEvent
    /// </summary>
    [Authorize(WorkspaceAccessEventPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetWorkspaceAccessEventLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.EventType
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent> ApplyFilters(IQueryable<Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent> queryable, WorkspaceAccessEventGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
