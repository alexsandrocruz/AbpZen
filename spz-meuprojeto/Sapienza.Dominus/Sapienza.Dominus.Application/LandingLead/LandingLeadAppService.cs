using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LandingLead.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LandingLead;

/// <summary>
/// Application service for LandingLead entity
/// </summary>
[Authorize(LandingLeadPermissions.Default)]
public class LandingLeadAppService :
    DominusAppService,
    ILandingLeadAppService
{
    private readonly IRepository<Dominus.LandingLead.LandingLead, Guid> _repository;

    public LandingLeadAppService(
        IRepository<Dominus.LandingLead.LandingLead, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single LandingLead by Id
    /// </summary>
    public virtual async Task<LandingLeadDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LandingLead.LandingLead, LandingLeadDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LandingLeads
    /// </summary>
    public virtual async Task<PagedResultDto<LandingLeadDto>> GetListAsync(LandingLeadGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LandingLead.LandingLead>, List<LandingLeadDto>>(entities);

        return new PagedResultDto<LandingLeadDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LandingLead
    /// </summary>
    [Authorize(LandingLeadPermissions.Create)]
    public virtual async Task<LandingLeadDto> CreateAsync(CreateUpdateLandingLeadDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLandingLeadDto, Dominus.LandingLead.LandingLead>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LandingLead.LandingLead, LandingLeadDto>(entity);
    }

    /// <summary>
    /// Updates an existing LandingLead
    /// </summary>
    [Authorize(LandingLeadPermissions.Update)]
    public virtual async Task<LandingLeadDto> UpdateAsync(Guid id, CreateUpdateLandingLeadDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LandingLead.LandingLead), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LandingLead.LandingLead, LandingLeadDto>(entity);
    }

    /// <summary>
    /// Deletes a LandingLead
    /// </summary>
    [Authorize(LandingLeadPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLandingLeadLookupAsync()
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
    protected virtual IQueryable<Dominus.LandingLead.LandingLead> ApplyFilters(IQueryable<Dominus.LandingLead.LandingLead> queryable, LandingLeadGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
