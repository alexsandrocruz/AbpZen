using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.EventoZen.Permissions;
using Sapienza.EventoZen.ArtistSpecialty.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.ArtistSpecialty;

/// <summary>
/// Application service for ArtistSpecialty entity
/// </summary>
[Authorize(ArtistSpecialtyPermissions.Default)]
public class ArtistSpecialtyAppService :
    EventoZenAppService,
    IArtistSpecialtyAppService
{
    private readonly IRepository<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty, Guid> _repository;
    private readonly IRepository<Sapienza.EventoZen.Artist.Artist, Guid> _artistRepository;

    public ArtistSpecialtyAppService(
        IRepository<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty, Guid> repository,
        IRepository<Sapienza.EventoZen.Artist.Artist, Guid> artistRepository
    )
    {
        _repository = repository;
        _artistRepository = artistRepository;
    }

    /// <summary>
    /// Gets a single ArtistSpecialty by Id
    /// </summary>
    public virtual async Task<ArtistSpecialtyDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty, ArtistSpecialtyDto>(entity);
        if (entity.ArtistId != null)
        {
            var parent = await _artistRepository.FindAsync(entity.ArtistId.Value);
            dto.ArtistDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of ArtistSpecialties
    /// </summary>
    public virtual async Task<PagedResultDto<ArtistSpecialtyDto>> GetListAsync(ArtistSpecialtyGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty>, List<ArtistSpecialtyDto>>(entities);
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

        return new PagedResultDto<ArtistSpecialtyDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new ArtistSpecialty
    /// </summary>
    [Authorize(ArtistSpecialtyPermissions.Create)]
    public virtual async Task<ArtistSpecialtyDto> CreateAsync(CreateUpdateArtistSpecialtyDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateArtistSpecialtyDto, Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty, ArtistSpecialtyDto>(entity);
    }

    /// <summary>
    /// Updates an existing ArtistSpecialty
    /// </summary>
    [Authorize(ArtistSpecialtyPermissions.Update)]
    public virtual async Task<ArtistSpecialtyDto> UpdateAsync(Guid id, CreateUpdateArtistSpecialtyDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty, ArtistSpecialtyDto>(entity);
    }

    /// <summary>
    /// Deletes a ArtistSpecialty
    /// </summary>
    [Authorize(ArtistSpecialtyPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetArtistSpecialtyLookupAsync()
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
    protected virtual IQueryable<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty> ApplyFilters(IQueryable<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty> queryable, ArtistSpecialtyGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.Name.Contains(input.Filter) || x.Description.Contains(input.Filter))
            .WhereIf(input.ArtistId != null, x => x.ArtistId == input.ArtistId)
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description.Contains(input.Description))
            // ========== FK Filters ==========
            .WhereIf(input.ArtistId != null, x => x.ArtistId == input.ArtistId)
            ;
    }
}
