using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.SmsLog.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SmsLog;

/// <summary>
/// Application service for SmsLog entity
/// </summary>
[Authorize(SmsLogPermissions.Default)]
public class SmsLogAppService :
    DominusAppService,
    ISmsLogAppService
{
    private readonly IRepository<Dominus.SmsLog.SmsLog, Guid> _repository;

    public SmsLogAppService(
        IRepository<Dominus.SmsLog.SmsLog, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single SmsLog by Id
    /// </summary>
    public virtual async Task<SmsLogDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.SmsLog.SmsLog, SmsLogDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of SmsLogs
    /// </summary>
    public virtual async Task<PagedResultDto<SmsLogDto>> GetListAsync(SmsLogGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.SmsLog.SmsLog>, List<SmsLogDto>>(entities);

        return new PagedResultDto<SmsLogDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new SmsLog
    /// </summary>
    [Authorize(SmsLogPermissions.Create)]
    public virtual async Task<SmsLogDto> CreateAsync(CreateUpdateSmsLogDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateSmsLogDto, Dominus.SmsLog.SmsLog>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SmsLog.SmsLog, SmsLogDto>(entity);
    }

    /// <summary>
    /// Updates an existing SmsLog
    /// </summary>
    [Authorize(SmsLogPermissions.Update)]
    public virtual async Task<SmsLogDto> UpdateAsync(Guid id, CreateUpdateSmsLogDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.SmsLog.SmsLog), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SmsLog.SmsLog, SmsLogDto>(entity);
    }

    /// <summary>
    /// Deletes a SmsLog
    /// </summary>
    [Authorize(SmsLogPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSmsLogLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.ToPhone
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.SmsLog.SmsLog> ApplyFilters(IQueryable<Dominus.SmsLog.SmsLog> queryable, SmsLogGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
