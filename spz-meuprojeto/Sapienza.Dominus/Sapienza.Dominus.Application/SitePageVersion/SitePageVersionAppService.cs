using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.SitePageVersion.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SitePageVersion;

/// <summary>
/// Application service for SitePageVersion entity
/// </summary>
[Authorize(SitePageVersionPermissions.Default)]
public class SitePageVersionAppService :
    DominusAppService,
    ISitePageVersionAppService
{
    private readonly IRepository<Dominus.SitePageVersion.SitePageVersion, Guid> _repository;
    private readonly IRepository<Dominus.SitePage.SitePage, Guid> _sitePageRepository;

    public SitePageVersionAppService(
        IRepository<Dominus.SitePageVersion.SitePageVersion, Guid> repository,
        IRepository<Dominus.SitePage.SitePage, Guid> sitePageRepository
    )
    {
        _repository = repository;
        _sitePageRepository = sitePageRepository;
    }

    /// <summary>
    /// Gets a single SitePageVersion by Id
    /// </summary>
    public virtual async Task<SitePageVersionDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.SitePageVersion.SitePageVersion, SitePageVersionDto>(entity);
        if (entity.SitePageId != null)
        {
            var parent = await _sitePageRepository.FindAsync(entity.SitePageId.Value);
            dto.SitePageDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of SitePageVersions
    /// </summary>
    public virtual async Task<PagedResultDto<SitePageVersionDto>> GetListAsync(SitePageVersionGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.SitePageVersion.SitePageVersion>, List<SitePageVersionDto>>(entities);
        var sitePageIds = entities
            .Where(x => x.SitePageId != null)
            .Select(x => x.SitePageId.Value)
            .Distinct()
            .ToList();

        if (sitePageIds.Any())
        {
            var parents = await _sitePageRepository.GetListAsync(x => sitePageIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Title);

            foreach (var dto in dtoList.Where(x => x.SitePageId != null))
            {
                if (parentMap.TryGetValue(dto.SitePageId.Value, out var displayName))
                {
                    dto.SitePageDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<SitePageVersionDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new SitePageVersion
    /// </summary>
    [Authorize(SitePageVersionPermissions.Create)]
    public virtual async Task<SitePageVersionDto> CreateAsync(CreateUpdateSitePageVersionDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateSitePageVersionDto, Dominus.SitePageVersion.SitePageVersion>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SitePageVersion.SitePageVersion, SitePageVersionDto>(entity);
    }

    /// <summary>
    /// Updates an existing SitePageVersion
    /// </summary>
    [Authorize(SitePageVersionPermissions.Update)]
    public virtual async Task<SitePageVersionDto> UpdateAsync(Guid id, CreateUpdateSitePageVersionDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.SitePageVersion.SitePageVersion), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SitePageVersion.SitePageVersion, SitePageVersionDto>(entity);
    }

    /// <summary>
    /// Deletes a SitePageVersion
    /// </summary>
    [Authorize(SitePageVersionPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSitePageVersionLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Id.ToString()
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.SitePageVersion.SitePageVersion> ApplyFilters(IQueryable<Dominus.SitePageVersion.SitePageVersion> queryable, SitePageVersionGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.SitePageId != null, x => x.SitePageId == input.SitePageId)
            ;
    }
}
