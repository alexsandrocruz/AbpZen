using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.WorkspaceUsageMetric.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WorkspaceUsageMetric;

/// <summary>
/// Application service for WorkspaceUsageMetric entity
/// </summary>
[Authorize(WorkspaceUsageMetricPermissions.Default)]
public class WorkspaceUsageMetricAppService :
    DominusAppService,
    IWorkspaceUsageMetricAppService
{
    private readonly IRepository<Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric, Guid> _repository;

    public WorkspaceUsageMetricAppService(
        IRepository<Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single WorkspaceUsageMetric by Id
    /// </summary>
    public virtual async Task<WorkspaceUsageMetricDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric, WorkspaceUsageMetricDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of WorkspaceUsageMetrics
    /// </summary>
    public virtual async Task<PagedResultDto<WorkspaceUsageMetricDto>> GetListAsync(WorkspaceUsageMetricGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric>, List<WorkspaceUsageMetricDto>>(entities);

        return new PagedResultDto<WorkspaceUsageMetricDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new WorkspaceUsageMetric
    /// </summary>
    [Authorize(WorkspaceUsageMetricPermissions.Create)]
    public virtual async Task<WorkspaceUsageMetricDto> CreateAsync(CreateUpdateWorkspaceUsageMetricDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateWorkspaceUsageMetricDto, Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric, WorkspaceUsageMetricDto>(entity);
    }

    /// <summary>
    /// Updates an existing WorkspaceUsageMetric
    /// </summary>
    [Authorize(WorkspaceUsageMetricPermissions.Update)]
    public virtual async Task<WorkspaceUsageMetricDto> UpdateAsync(Guid id, CreateUpdateWorkspaceUsageMetricDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric, WorkspaceUsageMetricDto>(entity);
    }

    /// <summary>
    /// Deletes a WorkspaceUsageMetric
    /// </summary>
    [Authorize(WorkspaceUsageMetricPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetWorkspaceUsageMetricLookupAsync()
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
    protected virtual IQueryable<Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric> ApplyFilters(IQueryable<Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric> queryable, WorkspaceUsageMetricGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
