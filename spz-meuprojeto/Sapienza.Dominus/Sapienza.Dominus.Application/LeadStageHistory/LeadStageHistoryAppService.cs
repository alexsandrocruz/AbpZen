using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadStageHistory.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadStageHistory;

/// <summary>
/// Application service for LeadStageHistory entity
/// </summary>
[Authorize(LeadStageHistoryPermissions.Default)]
public class LeadStageHistoryAppService :
    DominusAppService,
    ILeadStageHistoryAppService
{
    private readonly IRepository<Dominus.LeadStageHistory.LeadStageHistory, Guid> _repository;
    private readonly IRepository<Dominus.Lead.Lead, Guid> _leadRepository;

    public LeadStageHistoryAppService(
        IRepository<Dominus.LeadStageHistory.LeadStageHistory, Guid> repository,
        IRepository<Dominus.Lead.Lead, Guid> leadRepository
    )
    {
        _repository = repository;
        _leadRepository = leadRepository;
    }

    /// <summary>
    /// Gets a single LeadStageHistory by Id
    /// </summary>
    public virtual async Task<LeadStageHistoryDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadStageHistory.LeadStageHistory, LeadStageHistoryDto>(entity);
        if (entity.LeadId != null)
        {
            var parent = await _leadRepository.FindAsync(entity.LeadId.Value);
            dto.LeadDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadStageHistories
    /// </summary>
    public virtual async Task<PagedResultDto<LeadStageHistoryDto>> GetListAsync(LeadStageHistoryGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LeadStageHistory.LeadStageHistory>, List<LeadStageHistoryDto>>(entities);
        var leadIds = entities
            .Where(x => x.LeadId != null)
            .Select(x => x.LeadId.Value)
            .Distinct()
            .ToList();

        if (leadIds.Any())
        {
            var parents = await _leadRepository.GetListAsync(x => leadIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.LeadId != null))
            {
                if (parentMap.TryGetValue(dto.LeadId.Value, out var displayName))
                {
                    dto.LeadDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<LeadStageHistoryDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadStageHistory
    /// </summary>
    [Authorize(LeadStageHistoryPermissions.Create)]
    public virtual async Task<LeadStageHistoryDto> CreateAsync(CreateUpdateLeadStageHistoryDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadStageHistoryDto, Dominus.LeadStageHistory.LeadStageHistory>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadStageHistory.LeadStageHistory, LeadStageHistoryDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadStageHistory
    /// </summary>
    [Authorize(LeadStageHistoryPermissions.Update)]
    public virtual async Task<LeadStageHistoryDto> UpdateAsync(Guid id, CreateUpdateLeadStageHistoryDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadStageHistory.LeadStageHistory), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadStageHistory.LeadStageHistory, LeadStageHistoryDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadStageHistory
    /// </summary>
    [Authorize(LeadStageHistoryPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadStageHistoryLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Notes
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.LeadStageHistory.LeadStageHistory> ApplyFilters(IQueryable<Dominus.LeadStageHistory.LeadStageHistory> queryable, LeadStageHistoryGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.LeadId != null, x => x.LeadId == input.LeadId)
            ;
    }
}
