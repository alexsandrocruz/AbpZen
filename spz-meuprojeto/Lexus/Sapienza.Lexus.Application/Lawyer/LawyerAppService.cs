#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.Lawyer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.Lawyer;

/// <summary>
/// Application service for Lawyer entity
/// </summary>
[Authorize(LawyerPermissions.Default)]
public class LawyerAppService :
    LexusAppService,
    ILawyerAppService
{
    private readonly IRepository<Sapienza.Lexus.Lawyer.Lawyer, Guid> _repository;

    public LawyerAppService(
        IRepository<Sapienza.Lexus.Lawyer.Lawyer, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Lawyer by Id
    /// </summary>
    public virtual async Task<LawyerDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.Lawyer.Lawyer, LawyerDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Lawyers
    /// </summary>
    public virtual async Task<PagedResultDto<LawyerDto>> GetListAsync(LawyerGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.Lawyer.Lawyer>, List<LawyerDto>>(entities);

        return new PagedResultDto<LawyerDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Lawyer
    /// </summary>
    [Authorize(LawyerPermissions.Create)]
    public virtual async Task<LawyerDto> CreateAsync(CreateUpdateLawyerDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLawyerDto, Sapienza.Lexus.Lawyer.Lawyer>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.Lawyer.Lawyer, LawyerDto>(entity);
    }

    /// <summary>
    /// Updates an existing Lawyer
    /// </summary>
    [Authorize(LawyerPermissions.Update)]
    public virtual async Task<LawyerDto> UpdateAsync(Guid id, CreateUpdateLawyerDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.Lawyer.Lawyer), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.Lawyer.Lawyer, LawyerDto>(entity);
    }

    /// <summary>
    /// Deletes a Lawyer
    /// </summary>
    [Authorize(LawyerPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLawyerLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.FullName
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.Lawyer.Lawyer> ApplyFilters(IQueryable<Sapienza.Lexus.Lawyer.Lawyer> queryable, LawyerGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>(x.FullName != null && x.FullName.Contains(input.Filter)) || (x.PreferredName != null && x.PreferredName.Contains(input.Filter)))
            .WhereIf(!input.FullName.IsNullOrWhiteSpace(), x => x.FullName != null && x.FullName.Contains(input.FullName))
            .WhereIf(!input.PreferredName.IsNullOrWhiteSpace(), x => x.PreferredName != null && x.PreferredName.Contains(input.PreferredName))
            // ========== FK Filters ==========
            ;
    }
}
