using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Workflow.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Workflow;

/// <summary>
/// Application service for Workflow entity
/// </summary>
[Authorize(WorkflowPermissions.Default)]
public class WorkflowAppService :
    DominusAppService,
    IWorkflowAppService
{
    private readonly IRepository<Dominus.Workflow.Workflow, Guid> _repository;

    public WorkflowAppService(
        IRepository<Dominus.Workflow.Workflow, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Workflow by Id
    /// </summary>
    public virtual async Task<WorkflowDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Workflow.Workflow, WorkflowDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Workflows
    /// </summary>
    public virtual async Task<PagedResultDto<WorkflowDto>> GetListAsync(WorkflowGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Workflow.Workflow>, List<WorkflowDto>>(entities);

        return new PagedResultDto<WorkflowDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Workflow
    /// </summary>
    [Authorize(WorkflowPermissions.Create)]
    public virtual async Task<WorkflowDto> CreateAsync(CreateUpdateWorkflowDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateWorkflowDto, Dominus.Workflow.Workflow>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Workflow.Workflow, WorkflowDto>(entity);
    }

    /// <summary>
    /// Updates an existing Workflow
    /// </summary>
    [Authorize(WorkflowPermissions.Update)]
    public virtual async Task<WorkflowDto> UpdateAsync(Guid id, CreateUpdateWorkflowDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Workflow.Workflow), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Workflow.Workflow, WorkflowDto>(entity);
    }

    /// <summary>
    /// Deletes a Workflow
    /// </summary>
    [Authorize(WorkflowPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetWorkflowLookupAsync()
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
    protected virtual IQueryable<Dominus.Workflow.Workflow> ApplyFilters(IQueryable<Dominus.Workflow.Workflow> queryable, WorkflowGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
