using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Lead.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Lead;

/// <summary>
/// Application service for Lead entity
/// </summary>
[Authorize(LeadPermissions.Default)]
public class LeadAppService :
    DominusAppService,
    ILeadAppService
{
    private readonly IRepository<Dominus.Lead.Lead, Guid> _repository;
    private readonly IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> _leadWorkflowRepository;
    private readonly IRepository<Dominus.LeadWorkflowStage.LeadWorkflowStage, Guid> _leadWorkflowStageRepository;

    public LeadAppService(
        IRepository<Dominus.Lead.Lead, Guid> repository,
        IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> leadWorkflowRepository,
        IRepository<Dominus.LeadWorkflowStage.LeadWorkflowStage, Guid> leadWorkflowStageRepository
    )
    {
        _repository = repository;
        _leadWorkflowRepository = leadWorkflowRepository;
        _leadWorkflowStageRepository = leadWorkflowStageRepository;
    }

    /// <summary>
    /// Gets a single Lead by Id
    /// </summary>
    public virtual async Task<LeadDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Lead.Lead, LeadDto>(entity);
        if (entity.LeadWorkflowId != null)
        {
            var parent = await _leadWorkflowRepository.FindAsync(entity.LeadWorkflowId.Value);
            dto.LeadWorkflowDisplayName = parent?.Name;
        }
        if (entity.LeadWorkflowStageId != null)
        {
            var parent = await _leadWorkflowStageRepository.FindAsync(entity.LeadWorkflowStageId.Value);
            dto.LeadWorkflowStageDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Leads
    /// </summary>
    public virtual async Task<PagedResultDto<LeadDto>> GetListAsync(LeadGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Lead.Lead>, List<LeadDto>>(entities);
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
        var leadWorkflowStageIds = entities
            .Where(x => x.LeadWorkflowStageId != null)
            .Select(x => x.LeadWorkflowStageId.Value)
            .Distinct()
            .ToList();

        if (leadWorkflowStageIds.Any())
        {
            var parents = await _leadWorkflowStageRepository.GetListAsync(x => leadWorkflowStageIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.LeadWorkflowStageId != null))
            {
                if (parentMap.TryGetValue(dto.LeadWorkflowStageId.Value, out var displayName))
                {
                    dto.LeadWorkflowStageDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<LeadDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Lead
    /// </summary>
    [Authorize(LeadPermissions.Create)]
    public virtual async Task<LeadDto> CreateAsync(CreateUpdateLeadDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadDto, Dominus.Lead.Lead>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Lead.Lead, LeadDto>(entity);
    }

    /// <summary>
    /// Updates an existing Lead
    /// </summary>
    [Authorize(LeadPermissions.Update)]
    public virtual async Task<LeadDto> UpdateAsync(Guid id, CreateUpdateLeadDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Lead.Lead), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Lead.Lead, LeadDto>(entity);
    }

    /// <summary>
    /// Deletes a Lead
    /// </summary>
    [Authorize(LeadPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadLookupAsync()
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
    protected virtual IQueryable<Dominus.Lead.Lead> ApplyFilters(IQueryable<Dominus.Lead.Lead> queryable, LeadGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.LeadWorkflowId != null, x => x.LeadWorkflowId == input.LeadWorkflowId)
            .WhereIf(input.LeadWorkflowStageId != null, x => x.LeadWorkflowStageId == input.LeadWorkflowStageId)
            ;
    }
}
