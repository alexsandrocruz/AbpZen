using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.EventoZen.Permissions;
using Sapienza.EventoZen.Artist.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.Artist;

/// <summary>
/// Application service for Artist entity
/// </summary>
[Authorize(ArtistPermissions.Default)]
public class ArtistAppService :
    EventoZenAppService,
    IArtistAppService
{
    private readonly IRepository<Sapienza.EventoZen.Artist.Artist, Guid> _repository;

    public ArtistAppService(
        IRepository<Sapienza.EventoZen.Artist.Artist, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Artist by Id
    /// </summary>
    public virtual async Task<ArtistDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.EventoZen.Artist.Artist, ArtistDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Artists
    /// </summary>
    public virtual async Task<PagedResultDto<ArtistDto>> GetListAsync(ArtistGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.EventoZen.Artist.Artist>, List<ArtistDto>>(entities);

        return new PagedResultDto<ArtistDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Artist
    /// </summary>
    [Authorize(ArtistPermissions.Create)]
    public virtual async Task<ArtistDto> CreateAsync(CreateUpdateArtistDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateArtistDto, Sapienza.EventoZen.Artist.Artist>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.Artist.Artist, ArtistDto>(entity);
    }

    /// <summary>
    /// Updates an existing Artist
    /// </summary>
    [Authorize(ArtistPermissions.Update)]
    public virtual async Task<ArtistDto> UpdateAsync(Guid id, CreateUpdateArtistDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.EventoZen.Artist.Artist), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.Artist.Artist, ArtistDto>(entity);
    }

    /// <summary>
    /// Deletes a Artist
    /// </summary>
    [Authorize(ArtistPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetArtistLookupAsync()
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
    protected virtual IQueryable<Sapienza.EventoZen.Artist.Artist> ApplyFilters(IQueryable<Sapienza.EventoZen.Artist.Artist> queryable, ArtistGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.Name.Contains(input.Filter) || x.Biography.Contains(input.Filter) || x.PhotoUrl.Contains(input.Filter) || x.InstagramHandle.Contains(input.Filter) || x.WebsiteUrl.Contains(input.Filter) || x.LogoUrl.Contains(input.Filter) || x.BannerUrl.Contains(input.Filter) || x.HexColor.Contains(input.Filter))
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            .WhereIf(input.Type != null, x => x.Type == input.Type)
            .WhereIf(!input.Biography.IsNullOrWhiteSpace(), x => x.Biography.Contains(input.Biography))
            .WhereIf(!input.PhotoUrl.IsNullOrWhiteSpace(), x => x.PhotoUrl.Contains(input.PhotoUrl))
            .WhereIf(input.IsActive != null, x => x.IsActive == input.IsActive)
            .WhereIf(!input.InstagramHandle.IsNullOrWhiteSpace(), x => x.InstagramHandle.Contains(input.InstagramHandle))
            .WhereIf(!input.WebsiteUrl.IsNullOrWhiteSpace(), x => x.WebsiteUrl.Contains(input.WebsiteUrl))
            .WhereIf(!input.LogoUrl.IsNullOrWhiteSpace(), x => x.LogoUrl.Contains(input.LogoUrl))
            .WhereIf(!input.BannerUrl.IsNullOrWhiteSpace(), x => x.BannerUrl.Contains(input.BannerUrl))
            .WhereIf(!input.HexColor.IsNullOrWhiteSpace(), x => x.HexColor.Contains(input.HexColor))
            // ========== FK Filters ==========
            ;
    }
}
