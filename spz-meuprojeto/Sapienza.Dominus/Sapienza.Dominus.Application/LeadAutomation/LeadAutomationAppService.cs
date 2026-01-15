using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadAutomation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadAutomation;

/// <summary>
/// Application service for LeadAutomation entity
/// </summary>
[Authorize(LeadAutomationPermissions.Default)]
public class LeadAutomationAppService :
    DominusAppService,
    ILeadAutomationAppService
{
    private readonly IRepository<Dominus.LeadAutomation.LeadAutomation, Guid> _repository;
    private readonly IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> _leadWorkflowRepository;

    public LeadAutomationAppService(
        IRepository<Dominus.LeadAutomation.LeadAutomation, Guid> repository,
        IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> leadWorkflowRepository
    )
    {
        _repository = repository;
        _leadWorkflowRepository = leadWorkflowRepository;
    }

    /// <summary>
    /// Gets a single LeadAutomation by Id
    /// </summary>
    public virtual async Task<LeadAutomationDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadAutomation.LeadAutomation, LeadAutomationDto>(entity);
        if (entity.LeadWorkflowId != null)
        {
            var parent = await _leadWorkflowRepository.FindAsync(entity.LeadWorkflowId.Value);
            dto.LeadWorkflowDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadAutomations
    /// </summary>
    public virtual async Task<PagedResultDto<LeadAutomationDto>> GetListAsync(LeadAutomationGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LeadAutomation.LeadAutomation>, List<LeadAutomationDto>>(entities);
        var leadWorkflowIds = entities
            .Where(x => x.LeadWorkflowId != null)
            .Select(x => x.LeadWorkflowId.Value)
            .Distinct()
            .ToList();

        if (leadWorkflowIds.Any())
        {
            var parents = await _leadWorkflowRepository.GetListAsync(x => leadWorkflowIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.LeadWorkflowId != null))
            {
                if (parentMap.TryGetValue(dto.LeadWorkflowId.Value, out var displayName))
                {
                    dto.LeadWorkflowDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<LeadAutomationDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadAutomation
    /// </summary>
    [Authorize(LeadAutomationPermissions.Create)]
    public virtual async Task<LeadAutomationDto> CreateAsync(CreateUpdateLeadAutomationDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadAutomationDto, Dominus.LeadAutomation.LeadAutomation>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadAutomation.LeadAutomation, LeadAutomationDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadAutomation
    /// </summary>
    [Authorize(LeadAutomationPermissions.Update)]
    public virtual async Task<LeadAutomationDto> UpdateAsync(Guid id, CreateUpdateLeadAutomationDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadAutomation.LeadAutomation), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadAutomation.LeadAutomation, LeadAutomationDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadAutomation
    /// </summary>
    [Authorize(LeadAutomationPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadAutomationLookupAsync()
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
    protected virtual IQueryable<Dominus.LeadAutomation.LeadAutomation> ApplyFilters(IQueryable<Dominus.LeadAutomation.LeadAutomation> queryable, LeadAutomationGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.LeadWorkflowId != null, x => x.LeadWorkflowId == input.LeadWorkflowId)
            ;
    }
}
