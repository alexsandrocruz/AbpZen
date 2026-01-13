using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.EventoZen.Permissions;
using Sapienza.EventoZen.Availability.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.Availability;

/// <summary>
/// Application service for Availability entity
/// </summary>
[Authorize(AvailabilityPermissions.Default)]
public class AvailabilityAppService :
    EventoZenAppService,
    IAvailabilityAppService
{
    private readonly IRepository<Sapienza.EventoZen.Availability.Availability, Guid> _repository;
    private readonly IRepository<Sapienza.EventoZen.Artist.Artist, Guid> _artistRepository;

    public AvailabilityAppService(
        IRepository<Sapienza.EventoZen.Availability.Availability, Guid> repository,
        IRepository<Sapienza.EventoZen.Artist.Artist, Guid> artistRepository
    )
    {
        _repository = repository;
        _artistRepository = artistRepository;
    }

    /// <summary>
    /// Gets a single Availability by Id
    /// </summary>
    public virtual async Task<AvailabilityDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.EventoZen.Availability.Availability, AvailabilityDto>(entity);
        if (entity.ArtistId != null)
        {
            var parent = await _artistRepository.FindAsync(entity.ArtistId.Value);
            dto.ArtistDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Availabilities
    /// </summary>
    public virtual async Task<PagedResultDto<AvailabilityDto>> GetListAsync(AvailabilityGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.EventoZen.Availability.Availability>, List<AvailabilityDto>>(entities);
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

        return new PagedResultDto<AvailabilityDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Availability
    /// </summary>
    [Authorize(AvailabilityPermissions.Create)]
    public virtual async Task<AvailabilityDto> CreateAsync(CreateUpdateAvailabilityDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateAvailabilityDto, Sapienza.EventoZen.Availability.Availability>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.Availability.Availability, AvailabilityDto>(entity);
    }

    /// <summary>
    /// Updates an existing Availability
    /// </summary>
    [Authorize(AvailabilityPermissions.Update)]
    public virtual async Task<AvailabilityDto> UpdateAsync(Guid id, CreateUpdateAvailabilityDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.EventoZen.Availability.Availability), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.Availability.Availability, AvailabilityDto>(entity);
    }

    /// <summary>
    /// Deletes a Availability
    /// </summary>
    [Authorize(AvailabilityPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetAvailabilityLookupAsync()
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
    protected virtual IQueryable<Sapienza.EventoZen.Availability.Availability> ApplyFilters(IQueryable<Sapienza.EventoZen.Availability.Availability> queryable, AvailabilityGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.Notes.Contains(input.Filter))
            .WhereIf(input.Type != null, x => x.Type == input.Type)
            .WhereIf(input.ArtistId != null, x => x.ArtistId == input.ArtistId)
            .WhereIf(input.StartDate != null, x => x.StartDate == input.StartDate)
            .WhereIf(input.EndDate != null, x => x.EndDate == input.EndDate)
            .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes.Contains(input.Notes))
            // ========== FK Filters ==========
            .WhereIf(input.ArtistId != null, x => x.ArtistId == input.ArtistId)
            ;
    }
}
