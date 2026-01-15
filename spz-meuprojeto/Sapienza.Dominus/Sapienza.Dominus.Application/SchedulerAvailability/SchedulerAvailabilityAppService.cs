using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.SchedulerAvailability.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SchedulerAvailability;

/// <summary>
/// Application service for SchedulerAvailability entity
/// </summary>
[Authorize(SchedulerAvailabilityPermissions.Default)]
public class SchedulerAvailabilityAppService :
    DominusAppService,
    ISchedulerAvailabilityAppService
{
    private readonly IRepository<Dominus.SchedulerAvailability.SchedulerAvailability, Guid> _repository;
    private readonly IRepository<Dominus.SchedulerType.SchedulerType, Guid> _schedulerTypeRepository;

    public SchedulerAvailabilityAppService(
        IRepository<Dominus.SchedulerAvailability.SchedulerAvailability, Guid> repository,
        IRepository<Dominus.SchedulerType.SchedulerType, Guid> schedulerTypeRepository
    )
    {
        _repository = repository;
        _schedulerTypeRepository = schedulerTypeRepository;
    }

    /// <summary>
    /// Gets a single SchedulerAvailability by Id
    /// </summary>
    public virtual async Task<SchedulerAvailabilityDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.SchedulerAvailability.SchedulerAvailability, SchedulerAvailabilityDto>(entity);
        if (entity.SchedulerTypeId != null)
        {
            var parent = await _schedulerTypeRepository.FindAsync(entity.SchedulerTypeId.Value);
            dto.SchedulerTypeDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of SchedulerAvailabilities
    /// </summary>
    public virtual async Task<PagedResultDto<SchedulerAvailabilityDto>> GetListAsync(SchedulerAvailabilityGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.SchedulerAvailability.SchedulerAvailability>, List<SchedulerAvailabilityDto>>(entities);
        var schedulerTypeIds = entities
            .Where(x => x.SchedulerTypeId != null)
            .Select(x => x.SchedulerTypeId.Value)
            .Distinct()
            .ToList();

        if (schedulerTypeIds.Any())
        {
            var parents = await _schedulerTypeRepository.GetListAsync(x => schedulerTypeIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.SchedulerTypeId != null))
            {
                if (parentMap.TryGetValue(dto.SchedulerTypeId.Value, out var displayName))
                {
                    dto.SchedulerTypeDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<SchedulerAvailabilityDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new SchedulerAvailability
    /// </summary>
    [Authorize(SchedulerAvailabilityPermissions.Create)]
    public virtual async Task<SchedulerAvailabilityDto> CreateAsync(CreateUpdateSchedulerAvailabilityDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateSchedulerAvailabilityDto, Dominus.SchedulerAvailability.SchedulerAvailability>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SchedulerAvailability.SchedulerAvailability, SchedulerAvailabilityDto>(entity);
    }

    /// <summary>
    /// Updates an existing SchedulerAvailability
    /// </summary>
    [Authorize(SchedulerAvailabilityPermissions.Update)]
    public virtual async Task<SchedulerAvailabilityDto> UpdateAsync(Guid id, CreateUpdateSchedulerAvailabilityDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.SchedulerAvailability.SchedulerAvailability), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SchedulerAvailability.SchedulerAvailability, SchedulerAvailabilityDto>(entity);
    }

    /// <summary>
    /// Deletes a SchedulerAvailability
    /// </summary>
    [Authorize(SchedulerAvailabilityPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSchedulerAvailabilityLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.StartTime
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.SchedulerAvailability.SchedulerAvailability> ApplyFilters(IQueryable<Dominus.SchedulerAvailability.SchedulerAvailability> queryable, SchedulerAvailabilityGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.SchedulerTypeId != null, x => x.SchedulerTypeId == input.SchedulerTypeId)
            ;
    }
}
