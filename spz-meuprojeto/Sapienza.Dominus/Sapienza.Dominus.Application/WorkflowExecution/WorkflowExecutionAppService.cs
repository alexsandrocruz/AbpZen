using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.WorkflowExecution.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WorkflowExecution;

/// <summary>
/// Application service for WorkflowExecution entity
/// </summary>
[Authorize(WorkflowExecutionPermissions.Default)]
public class WorkflowExecutionAppService :
    DominusAppService,
    IWorkflowExecutionAppService
{
    private readonly IRepository<Dominus.WorkflowExecution.WorkflowExecution, Guid> _repository;
    private readonly IRepository<Dominus.Workflow.Workflow, Guid> _workflowRepository;

    public WorkflowExecutionAppService(
        IRepository<Dominus.WorkflowExecution.WorkflowExecution, Guid> repository,
        IRepository<Dominus.Workflow.Workflow, Guid> workflowRepository
    )
    {
        _repository = repository;
        _workflowRepository = workflowRepository;
    }

    /// <summary>
    /// Gets a single WorkflowExecution by Id
    /// </summary>
    public virtual async Task<WorkflowExecutionDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.WorkflowExecution.WorkflowExecution, WorkflowExecutionDto>(entity);
        if (entity.WorkflowId != null)
        {
            var parent = await _workflowRepository.FindAsync(entity.WorkflowId.Value);
            dto.WorkflowDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of WorkflowExecutions
    /// </summary>
    public virtual async Task<PagedResultDto<WorkflowExecutionDto>> GetListAsync(WorkflowExecutionGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.WorkflowExecution.WorkflowExecution>, List<WorkflowExecutionDto>>(entities);
        var workflowIds = entities
            .Where(x => x.WorkflowId != null)
            .Select(x => x.WorkflowId.Value)
            .Distinct()
            .ToList();

        if (workflowIds.Any())
        {
            var parents = await _workflowRepository.GetListAsync(x => workflowIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.WorkflowId != null))
            {
                if (parentMap.TryGetValue(dto.WorkflowId.Value, out var displayName))
                {
                    dto.WorkflowDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<WorkflowExecutionDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new WorkflowExecution
    /// </summary>
    [Authorize(WorkflowExecutionPermissions.Create)]
    public virtual async Task<WorkflowExecutionDto> CreateAsync(CreateUpdateWorkflowExecutionDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateWorkflowExecutionDto, Dominus.WorkflowExecution.WorkflowExecution>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.WorkflowExecution.WorkflowExecution, WorkflowExecutionDto>(entity);
    }

    /// <summary>
    /// Updates an existing WorkflowExecution
    /// </summary>
    [Authorize(WorkflowExecutionPermissions.Update)]
    public virtual async Task<WorkflowExecutionDto> UpdateAsync(Guid id, CreateUpdateWorkflowExecutionDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.WorkflowExecution.WorkflowExecution), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.WorkflowExecution.WorkflowExecution, WorkflowExecutionDto>(entity);
    }

    /// <summary>
    /// Deletes a WorkflowExecution
    /// </summary>
    [Authorize(WorkflowExecutionPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetWorkflowExecutionLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Status
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.WorkflowExecution.WorkflowExecution> ApplyFilters(IQueryable<Dominus.WorkflowExecution.WorkflowExecution> queryable, WorkflowExecutionGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.WorkflowId != null, x => x.WorkflowId == input.WorkflowId)
            ;
    }
}
