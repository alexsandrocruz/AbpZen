using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadWorkflow.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadWorkflow;

/// <summary>
/// Application service for LeadWorkflow entity
/// </summary>
[Authorize(LeadWorkflowPermissions.Default)]
public class LeadWorkflowAppService :
    DominusAppService,
    ILeadWorkflowAppService
{
    private readonly IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> _repository;

    public LeadWorkflowAppService(
        IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single LeadWorkflow by Id
    /// </summary>
    public virtual async Task<LeadWorkflowDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadWorkflow.LeadWorkflow, LeadWorkflowDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadWorkflows
    /// </summary>
    public virtual async Task<PagedResultDto<LeadWorkflowDto>> GetListAsync(LeadWorkflowGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LeadWorkflow.LeadWorkflow>, List<LeadWorkflowDto>>(entities);

        return new PagedResultDto<LeadWorkflowDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadWorkflow
    /// </summary>
    [Authorize(LeadWorkflowPermissions.Create)]
    public virtual async Task<LeadWorkflowDto> CreateAsync(CreateUpdateLeadWorkflowDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadWorkflowDto, Dominus.LeadWorkflow.LeadWorkflow>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadWorkflow.LeadWorkflow, LeadWorkflowDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadWorkflow
    /// </summary>
    [Authorize(LeadWorkflowPermissions.Update)]
    public virtual async Task<LeadWorkflowDto> UpdateAsync(Guid id, CreateUpdateLeadWorkflowDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadWorkflow.LeadWorkflow), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadWorkflow.LeadWorkflow, LeadWorkflowDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadWorkflow
    /// </summary>
    [Authorize(LeadWorkflowPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadWorkflowLookupAsync()
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
    protected virtual IQueryable<Dominus.LeadWorkflow.LeadWorkflow> ApplyFilters(IQueryable<Dominus.LeadWorkflow.LeadWorkflow> queryable, LeadWorkflowGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
