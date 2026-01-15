using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadWorkflowStage.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadWorkflowStage;

/// <summary>
/// Application service for LeadWorkflowStage entity
/// </summary>
[Authorize(LeadWorkflowStagePermissions.Default)]
public class LeadWorkflowStageAppService :
    DominusAppService,
    ILeadWorkflowStageAppService
{
    private readonly IRepository<Dominus.LeadWorkflowStage.LeadWorkflowStage, Guid> _repository;
    private readonly IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> _leadWorkflowRepository;

    public LeadWorkflowStageAppService(
        IRepository<Dominus.LeadWorkflowStage.LeadWorkflowStage, Guid> repository,
        IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> leadWorkflowRepository
    )
    {
        _repository = repository;
        _leadWorkflowRepository = leadWorkflowRepository;
    }

    /// <summary>
    /// Gets a single LeadWorkflowStage by Id
    /// </summary>
    public virtual async Task<LeadWorkflowStageDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadWorkflowStage.LeadWorkflowStage, LeadWorkflowStageDto>(entity);
        if (entity.LeadWorkflowId != null)
        {
            var parent = await _leadWorkflowRepository.FindAsync(entity.LeadWorkflowId.Value);
            dto.LeadWorkflowDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadWorkflowStages
    /// </summary>
    public virtual async Task<PagedResultDto<LeadWorkflowStageDto>> GetListAsync(LeadWorkflowStageGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LeadWorkflowStage.LeadWorkflowStage>, List<LeadWorkflowStageDto>>(entities);
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

        return new PagedResultDto<LeadWorkflowStageDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadWorkflowStage
    /// </summary>
    [Authorize(LeadWorkflowStagePermissions.Create)]
    public virtual async Task<LeadWorkflowStageDto> CreateAsync(CreateUpdateLeadWorkflowStageDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadWorkflowStageDto, Dominus.LeadWorkflowStage.LeadWorkflowStage>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadWorkflowStage.LeadWorkflowStage, LeadWorkflowStageDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadWorkflowStage
    /// </summary>
    [Authorize(LeadWorkflowStagePermissions.Update)]
    public virtual async Task<LeadWorkflowStageDto> UpdateAsync(Guid id, CreateUpdateLeadWorkflowStageDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadWorkflowStage.LeadWorkflowStage), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadWorkflowStage.LeadWorkflowStage, LeadWorkflowStageDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadWorkflowStage
    /// </summary>
    [Authorize(LeadWorkflowStagePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadWorkflowStageLookupAsync()
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
    protected virtual IQueryable<Dominus.LeadWorkflowStage.LeadWorkflowStage> ApplyFilters(IQueryable<Dominus.LeadWorkflowStage.LeadWorkflowStage> queryable, LeadWorkflowStageGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.LeadWorkflowId != null, x => x.LeadWorkflowId == input.LeadWorkflowId)
            ;
    }
}
