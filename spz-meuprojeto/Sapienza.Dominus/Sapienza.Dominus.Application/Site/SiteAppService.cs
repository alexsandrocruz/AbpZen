using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Site.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Site;

/// <summary>
/// Application service for Site entity
/// </summary>
[Authorize(SitePermissions.Default)]
public class SiteAppService :
    DominusAppService,
    ISiteAppService
{
    private readonly IRepository<Dominus.Site.Site, Guid> _repository;

    public SiteAppService(
        IRepository<Dominus.Site.Site, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Site by Id
    /// </summary>
    public virtual async Task<SiteDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Site.Site, SiteDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Sites
    /// </summary>
    public virtual async Task<PagedResultDto<SiteDto>> GetListAsync(SiteGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Site.Site>, List<SiteDto>>(entities);

        return new PagedResultDto<SiteDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Site
    /// </summary>
    [Authorize(SitePermissions.Create)]
    public virtual async Task<SiteDto> CreateAsync(CreateUpdateSiteDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateSiteDto, Dominus.Site.Site>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Site.Site, SiteDto>(entity);
    }

    /// <summary>
    /// Updates an existing Site
    /// </summary>
    [Authorize(SitePermissions.Update)]
    public virtual async Task<SiteDto> UpdateAsync(Guid id, CreateUpdateSiteDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Site.Site), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Site.Site, SiteDto>(entity);
    }

    /// <summary>
    /// Deletes a Site
    /// </summary>
    [Authorize(SitePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSiteLookupAsync()
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
    protected virtual IQueryable<Dominus.Site.Site> ApplyFilters(IQueryable<Dominus.Site.Site> queryable, SiteGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
