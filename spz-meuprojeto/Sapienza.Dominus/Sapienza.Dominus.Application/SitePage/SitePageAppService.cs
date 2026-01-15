using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.SitePage.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SitePage;

/// <summary>
/// Application service for SitePage entity
/// </summary>
[Authorize(SitePagePermissions.Default)]
public class SitePageAppService :
    DominusAppService,
    ISitePageAppService
{
    private readonly IRepository<Dominus.SitePage.SitePage, Guid> _repository;
    private readonly IRepository<Dominus.Site.Site, Guid> _siteRepository;

    public SitePageAppService(
        IRepository<Dominus.SitePage.SitePage, Guid> repository,
        IRepository<Dominus.Site.Site, Guid> siteRepository
    )
    {
        _repository = repository;
        _siteRepository = siteRepository;
    }

    /// <summary>
    /// Gets a single SitePage by Id
    /// </summary>
    public virtual async Task<SitePageDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.SitePage.SitePage, SitePageDto>(entity);
        if (entity.SiteId != null)
        {
            var parent = await _siteRepository.FindAsync(entity.SiteId.Value);
            dto.SiteDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of SitePages
    /// </summary>
    public virtual async Task<PagedResultDto<SitePageDto>> GetListAsync(SitePageGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.SitePage.SitePage>, List<SitePageDto>>(entities);
        var siteIds = entities
            .Where(x => x.SiteId != null)
            .Select(x => x.SiteId.Value)
            .Distinct()
            .ToList();

        if (siteIds.Any())
        {
            var parents = await _siteRepository.GetListAsync(x => siteIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.SiteId != null))
            {
                if (parentMap.TryGetValue(dto.SiteId.Value, out var displayName))
                {
                    dto.SiteDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<SitePageDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new SitePage
    /// </summary>
    [Authorize(SitePagePermissions.Create)]
    public virtual async Task<SitePageDto> CreateAsync(CreateUpdateSitePageDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateSitePageDto, Dominus.SitePage.SitePage>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SitePage.SitePage, SitePageDto>(entity);
    }

    /// <summary>
    /// Updates an existing SitePage
    /// </summary>
    [Authorize(SitePagePermissions.Update)]
    public virtual async Task<SitePageDto> UpdateAsync(Guid id, CreateUpdateSitePageDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.SitePage.SitePage), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SitePage.SitePage, SitePageDto>(entity);
    }

    /// <summary>
    /// Deletes a SitePage
    /// </summary>
    [Authorize(SitePagePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSitePageLookupAsync()
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
    protected virtual IQueryable<Dominus.SitePage.SitePage> ApplyFilters(IQueryable<Dominus.SitePage.SitePage> queryable, SitePageGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.SiteId != null, x => x.SiteId == input.SiteId)
            ;
    }
}
