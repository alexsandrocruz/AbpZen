using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadTag.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadTag;

/// <summary>
/// Application service for LeadTag entity
/// </summary>
[Authorize(LeadTagPermissions.Default)]
public class LeadTagAppService :
    DominusAppService,
    ILeadTagAppService
{
    private readonly IRepository<Dominus.LeadTag.LeadTag, Guid> _repository;

    public LeadTagAppService(
        IRepository<Dominus.LeadTag.LeadTag, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single LeadTag by Id
    /// </summary>
    public virtual async Task<LeadTagDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadTag.LeadTag, LeadTagDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadTags
    /// </summary>
    public virtual async Task<PagedResultDto<LeadTagDto>> GetListAsync(LeadTagGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LeadTag.LeadTag>, List<LeadTagDto>>(entities);

        return new PagedResultDto<LeadTagDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadTag
    /// </summary>
    [Authorize(LeadTagPermissions.Create)]
    public virtual async Task<LeadTagDto> CreateAsync(CreateUpdateLeadTagDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadTagDto, Dominus.LeadTag.LeadTag>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadTag.LeadTag, LeadTagDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadTag
    /// </summary>
    [Authorize(LeadTagPermissions.Update)]
    public virtual async Task<LeadTagDto> UpdateAsync(Guid id, CreateUpdateLeadTagDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadTag.LeadTag), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadTag.LeadTag, LeadTagDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadTag
    /// </summary>
    [Authorize(LeadTagPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadTagLookupAsync()
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
    protected virtual IQueryable<Dominus.LeadTag.LeadTag> ApplyFilters(IQueryable<Dominus.LeadTag.LeadTag> queryable, LeadTagGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
