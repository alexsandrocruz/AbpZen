using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.SiteVisitDailyStat.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SiteVisitDailyStat;

/// <summary>
/// Application service for SiteVisitDailyStat entity
/// </summary>
[Authorize(SiteVisitDailyStatPermissions.Default)]
public class SiteVisitDailyStatAppService :
    DominusAppService,
    ISiteVisitDailyStatAppService
{
    private readonly IRepository<Dominus.SiteVisitDailyStat.SiteVisitDailyStat, Guid> _repository;
    private readonly IRepository<Dominus.Site.Site, Guid> _siteRepository;

    public SiteVisitDailyStatAppService(
        IRepository<Dominus.SiteVisitDailyStat.SiteVisitDailyStat, Guid> repository,
        IRepository<Dominus.Site.Site, Guid> siteRepository
    )
    {
        _repository = repository;
        _siteRepository = siteRepository;
    }

    /// <summary>
    /// Gets a single SiteVisitDailyStat by Id
    /// </summary>
    public virtual async Task<SiteVisitDailyStatDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.SiteVisitDailyStat.SiteVisitDailyStat, SiteVisitDailyStatDto>(entity);
        if (entity.SiteId != null)
        {
            var parent = await _siteRepository.FindAsync(entity.SiteId.Value);
            dto.SiteDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of SiteVisitDailyStats
    /// </summary>
    public virtual async Task<PagedResultDto<SiteVisitDailyStatDto>> GetListAsync(SiteVisitDailyStatGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.SiteVisitDailyStat.SiteVisitDailyStat>, List<SiteVisitDailyStatDto>>(entities);
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

        return new PagedResultDto<SiteVisitDailyStatDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new SiteVisitDailyStat
    /// </summary>
    [Authorize(SiteVisitDailyStatPermissions.Create)]
    public virtual async Task<SiteVisitDailyStatDto> CreateAsync(CreateUpdateSiteVisitDailyStatDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateSiteVisitDailyStatDto, Dominus.SiteVisitDailyStat.SiteVisitDailyStat>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SiteVisitDailyStat.SiteVisitDailyStat, SiteVisitDailyStatDto>(entity);
    }

    /// <summary>
    /// Updates an existing SiteVisitDailyStat
    /// </summary>
    [Authorize(SiteVisitDailyStatPermissions.Update)]
    public virtual async Task<SiteVisitDailyStatDto> UpdateAsync(Guid id, CreateUpdateSiteVisitDailyStatDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.SiteVisitDailyStat.SiteVisitDailyStat), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SiteVisitDailyStat.SiteVisitDailyStat, SiteVisitDailyStatDto>(entity);
    }

    /// <summary>
    /// Deletes a SiteVisitDailyStat
    /// </summary>
    [Authorize(SiteVisitDailyStatPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSiteVisitDailyStatLookupAsync()
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
    protected virtual IQueryable<Dominus.SiteVisitDailyStat.SiteVisitDailyStat> ApplyFilters(IQueryable<Dominus.SiteVisitDailyStat.SiteVisitDailyStat> queryable, SiteVisitDailyStatGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.SiteId != null, x => x.SiteId == input.SiteId)
            ;
    }
}
