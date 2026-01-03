using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.Lead.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Lead;

/// <summary>
/// Application service for Lead entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.Lead.Default)]
public class LeadAppService :
    LeptonXDemoAppAppService,
    ILeadAppService
{
    private readonly IRepository<LeptonXDemoApp.Lead.Lead, Guid> _repository;

    public LeadAppService(
        IRepository<LeptonXDemoApp.Lead.Lead, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Lead by Id
    /// </summary>
    public virtual async Task<LeadDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<LeptonXDemoApp.Lead.Lead, LeadDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Leads
    /// </summary>
    public virtual async Task<PagedResultDto<LeadDto>> GetListAsync(LeadGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.Lead.Lead>, List<LeadDto>>(entities);

        return new PagedResultDto<LeadDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Lead
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Lead.Create)]
    public virtual async Task<LeadDto> CreateAsync(CreateUpdateLeadDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadDto, LeptonXDemoApp.Lead.Lead>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Lead.Lead, LeadDto>(entity);
    }

    /// <summary>
    /// Updates an existing Lead
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Lead.Update)]
    public virtual async Task<LeadDto> UpdateAsync(Guid id, CreateUpdateLeadDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.Lead.Lead), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Lead.Lead, LeadDto>(entity);
    }

    /// <summary>
    /// Deletes a Lead
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Lead.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadLookupAsync()
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
    protected virtual IQueryable<LeptonXDemoApp.Lead.Lead> ApplyFilters(IQueryable<LeptonXDemoApp.Lead.Lead> queryable, LeadGetListInput input)
    {
        return queryable
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            .WhereIf(!input.Email.IsNullOrWhiteSpace(), x => x.Email.Contains(input.Email))
            .WhereIf(!input.Phone.IsNullOrWhiteSpace(), x => x.Phone.Contains(input.Phone))
            // ========== FK Filters ==========
            ;
    }
}
