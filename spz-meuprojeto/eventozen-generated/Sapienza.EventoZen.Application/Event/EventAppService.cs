using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.EventoZen.Permissions;
using Sapienza.EventoZen.Event.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.Event;

/// <summary>
/// Application service for Event entity
/// </summary>
[Authorize(EventPermissions.Default)]
public class EventAppService :
    EventoZenAppService,
    IEventAppService
{
    private readonly IRepository<Sapienza.EventoZen.Event.Event, Guid> _repository;
    private readonly IRepository<Sapienza.EventoZen.Artist.Artist, Guid> _artistRepository;
    private readonly IRepository<Sapienza.EventoZen.Client.Client, Guid> _clientRepository;
    private readonly IRepository<Sapienza.EventoZen.Location.Location, Guid> _locationRepository;

    public EventAppService(
        IRepository<Sapienza.EventoZen.Event.Event, Guid> repository,
        IRepository<Sapienza.EventoZen.Artist.Artist, Guid> artistRepository,
        IRepository<Sapienza.EventoZen.Client.Client, Guid> clientRepository,
        IRepository<Sapienza.EventoZen.Location.Location, Guid> locationRepository
    )
    {
        _repository = repository;
        _artistRepository = artistRepository;
        _clientRepository = clientRepository;
        _locationRepository = locationRepository;
    }

    /// <summary>
    /// Gets a single Event by Id
    /// </summary>
    public virtual async Task<EventDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.EventoZen.Event.Event, EventDto>(entity);
        if (entity.ArtistId != null)
        {
            var parent = await _artistRepository.FindAsync(entity.ArtistId.Value);
            dto.ArtistDisplayName = parent?.Name;
        }
        if (entity.ClientId != null)
        {
            var parent = await _clientRepository.FindAsync(entity.ClientId.Value);
            dto.ClientDisplayName = parent?.Name;
        }
        if (entity.LocationId != null)
        {
            var parent = await _locationRepository.FindAsync(entity.LocationId.Value);
            dto.LocationDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Events
    /// </summary>
    public virtual async Task<PagedResultDto<EventDto>> GetListAsync(EventGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.EventoZen.Event.Event>, List<EventDto>>(entities);
        var artistIds = entities
            .Where(x => x.ArtistId != null)
            .Select(x => x.ArtistId.Value)
            .Distinct()
            .ToList();

        if (artistIds.Any())
        {
            var parents = await _artistRepository.GetListAsync(x => artistIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.ArtistId != null))
            {
                if (parentMap.TryGetValue(dto.ArtistId.Value, out var displayName))
                {
                    dto.ArtistDisplayName = displayName;
                }
            }
        }
        var clientIds = entities
            .Where(x => x.ClientId != null)
            .Select(x => x.ClientId.Value)
            .Distinct()
            .ToList();

        if (clientIds.Any())
        {
            var parents = await _clientRepository.GetListAsync(x => clientIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.ClientId != null))
            {
                if (parentMap.TryGetValue(dto.ClientId.Value, out var displayName))
                {
                    dto.ClientDisplayName = displayName;
                }
            }
        }
        var locationIds = entities
            .Where(x => x.LocationId != null)
            .Select(x => x.LocationId.Value)
            .Distinct()
            .ToList();

        if (locationIds.Any())
        {
            var parents = await _locationRepository.GetListAsync(x => locationIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.LocationId != null))
            {
                if (parentMap.TryGetValue(dto.LocationId.Value, out var displayName))
                {
                    dto.LocationDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<EventDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Event
    /// </summary>
    [Authorize(EventPermissions.Create)]
    public virtual async Task<EventDto> CreateAsync(CreateUpdateEventDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateEventDto, Sapienza.EventoZen.Event.Event>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.Event.Event, EventDto>(entity);
    }

    /// <summary>
    /// Updates an existing Event
    /// </summary>
    [Authorize(EventPermissions.Update)]
    public virtual async Task<EventDto> UpdateAsync(Guid id, CreateUpdateEventDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.EventoZen.Event.Event), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.Event.Event, EventDto>(entity);
    }

    /// <summary>
    /// Deletes a Event
    /// </summary>
    [Authorize(EventPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetEventLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Title
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.EventoZen.Event.Event> ApplyFilters(IQueryable<Sapienza.EventoZen.Event.Event> queryable, EventGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.Title.Contains(input.Filter) || x.Description.Contains(input.Filter) || x.ContractType.Contains(input.Filter) || x.NegotiationType.Contains(input.Filter))
            .WhereIf(input.ArtistId != null, x => x.ArtistId == input.ArtistId)
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            .WhereIf(input.LocalPartnerId != null, x => x.LocalPartnerId == input.LocalPartnerId)
            .WhereIf(input.LocationId != null, x => x.LocationId == input.LocationId)
            .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title.Contains(input.Title))
            .WhereIf(input.Type != null, x => x.Type == input.Type)
            .WhereIf(input.Status != null, x => x.Status == input.Status)
            .WhereIf(input.StartDateTime != null, x => x.StartDateTime == input.StartDateTime)
            .WhereIf(input.EndDateTime != null, x => x.EndDateTime == input.EndDateTime)
            .WhereIf(input.Fee != null, x => x.Fee == input.Fee)
            .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description.Contains(input.Description))
            .WhereIf(input.TaxPercentage != null, x => x.TaxPercentage == input.TaxPercentage)
            .WhereIf(input.TaxValue != null, x => x.TaxValue == input.TaxValue)
            .WhereIf(!input.ContractType.IsNullOrWhiteSpace(), x => x.ContractType.Contains(input.ContractType))
            .WhereIf(!input.NegotiationType.IsNullOrWhiteSpace(), x => x.NegotiationType.Contains(input.NegotiationType))
            .WhereIf(input.HasConflict != null, x => x.HasConflict == input.HasConflict)
            .WhereIf(input.SuggestedAlternativeArtistId != null, x => x.SuggestedAlternativeArtistId == input.SuggestedAlternativeArtistId)
            // ========== FK Filters ==========
            .WhereIf(input.ArtistId != null, x => x.ArtistId == input.ArtistId)
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            .WhereIf(input.LocationId != null, x => x.LocationId == input.LocationId)
            ;
    }
}
