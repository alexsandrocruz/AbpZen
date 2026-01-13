using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.EventoZen.Permissions;
using Sapienza.EventoZen.EventCommission.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.EventCommission;

/// <summary>
/// Application service for EventCommission entity
/// </summary>
[Authorize(EventCommissionPermissions.Default)]
public class EventCommissionAppService :
    EventoZenAppService,
    IEventCommissionAppService
{
    private readonly IRepository<Sapienza.EventoZen.EventCommission.EventCommission, Guid> _repository;
    private readonly IRepository<Sapienza.EventoZen.Event.Event, Guid> _eventRepository;

    public EventCommissionAppService(
        IRepository<Sapienza.EventoZen.EventCommission.EventCommission, Guid> repository,
        IRepository<Sapienza.EventoZen.Event.Event, Guid> eventRepository
    )
    {
        _repository = repository;
        _eventRepository = eventRepository;
    }

    /// <summary>
    /// Gets a single EventCommission by Id
    /// </summary>
    public virtual async Task<EventCommissionDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.EventoZen.EventCommission.EventCommission, EventCommissionDto>(entity);
        if (entity.EventId != null)
        {
            var parent = await _eventRepository.FindAsync(entity.EventId.Value);
            dto.EventDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of EventCommissions
    /// </summary>
    public virtual async Task<PagedResultDto<EventCommissionDto>> GetListAsync(EventCommissionGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.EventoZen.EventCommission.EventCommission>, List<EventCommissionDto>>(entities);
        var eventIds = entities
            .Where(x => x.EventId != null)
            .Select(x => x.EventId.Value)
            .Distinct()
            .ToList();

        if (eventIds.Any())
        {
            var parents = await _eventRepository.GetListAsync(x => eventIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Title);

            foreach (var dto in dtoList.Where(x => x.EventId != null))
            {
                if (parentMap.TryGetValue(dto.EventId.Value, out var displayName))
                {
                    dto.EventDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<EventCommissionDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new EventCommission
    /// </summary>
    [Authorize(EventCommissionPermissions.Create)]
    public virtual async Task<EventCommissionDto> CreateAsync(CreateUpdateEventCommissionDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateEventCommissionDto, Sapienza.EventoZen.EventCommission.EventCommission>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.EventCommission.EventCommission, EventCommissionDto>(entity);
    }

    /// <summary>
    /// Updates an existing EventCommission
    /// </summary>
    [Authorize(EventCommissionPermissions.Update)]
    public virtual async Task<EventCommissionDto> UpdateAsync(Guid id, CreateUpdateEventCommissionDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.EventoZen.EventCommission.EventCommission), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.EventCommission.EventCommission, EventCommissionDto>(entity);
    }

    /// <summary>
    /// Deletes a EventCommission
    /// </summary>
    [Authorize(EventCommissionPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetEventCommissionLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Description
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.EventoZen.EventCommission.EventCommission> ApplyFilters(IQueryable<Sapienza.EventoZen.EventCommission.EventCommission> queryable, EventCommissionGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.Description.Contains(input.Filter))
            .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description.Contains(input.Description))
            .WhereIf(input.Value != null, x => x.Value == input.Value)
            .WhereIf(input.Percentage != null, x => x.Percentage == input.Percentage)
            .WhereIf(input.EventId != null, x => x.EventId == input.EventId)
            // ========== FK Filters ==========
            .WhereIf(input.EventId != null, x => x.EventId == input.EventId)
            ;
    }
}
