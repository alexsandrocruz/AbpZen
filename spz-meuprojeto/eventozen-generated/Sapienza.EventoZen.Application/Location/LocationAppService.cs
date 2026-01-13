using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.EventoZen.Permissions;
using Sapienza.EventoZen.Location.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.Location;

/// <summary>
/// Application service for Location entity
/// </summary>
[Authorize(LocationPermissions.Default)]
public class LocationAppService :
    EventoZenAppService,
    ILocationAppService
{
    private readonly IRepository<Sapienza.EventoZen.Location.Location, Guid> _repository;

    public LocationAppService(
        IRepository<Sapienza.EventoZen.Location.Location, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Location by Id
    /// </summary>
    public virtual async Task<LocationDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.EventoZen.Location.Location, LocationDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Locations
    /// </summary>
    public virtual async Task<PagedResultDto<LocationDto>> GetListAsync(LocationGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.EventoZen.Location.Location>, List<LocationDto>>(entities);

        return new PagedResultDto<LocationDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Location
    /// </summary>
    [Authorize(LocationPermissions.Create)]
    public virtual async Task<LocationDto> CreateAsync(CreateUpdateLocationDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLocationDto, Sapienza.EventoZen.Location.Location>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.Location.Location, LocationDto>(entity);
    }

    /// <summary>
    /// Updates an existing Location
    /// </summary>
    [Authorize(LocationPermissions.Update)]
    public virtual async Task<LocationDto> UpdateAsync(Guid id, CreateUpdateLocationDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.EventoZen.Location.Location), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.Location.Location, LocationDto>(entity);
    }

    /// <summary>
    /// Deletes a Location
    /// </summary>
    [Authorize(LocationPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLocationLookupAsync()
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
    protected virtual IQueryable<Sapienza.EventoZen.Location.Location> ApplyFilters(IQueryable<Sapienza.EventoZen.Location.Location> queryable, LocationGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.Name.Contains(input.Filter) || x.Address.Contains(input.Filter) || x.City.Contains(input.Filter) || x.State.Contains(input.Filter) || x.ZipCode.Contains(input.Filter) || x.Notes.Contains(input.Filter))
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            .WhereIf(!input.Address.IsNullOrWhiteSpace(), x => x.Address.Contains(input.Address))
            .WhereIf(!input.City.IsNullOrWhiteSpace(), x => x.City.Contains(input.City))
            .WhereIf(!input.State.IsNullOrWhiteSpace(), x => x.State.Contains(input.State))
            .WhereIf(input.Capacity != null, x => x.Capacity == input.Capacity)
            .WhereIf(!input.ZipCode.IsNullOrWhiteSpace(), x => x.ZipCode.Contains(input.ZipCode))
            .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes.Contains(input.Notes))
            // ========== FK Filters ==========
            ;
    }
}
