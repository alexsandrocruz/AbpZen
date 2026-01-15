using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.WhatsappLog.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WhatsappLog;

/// <summary>
/// Application service for WhatsappLog entity
/// </summary>
[Authorize(WhatsappLogPermissions.Default)]
public class WhatsappLogAppService :
    DominusAppService,
    IWhatsappLogAppService
{
    private readonly IRepository<Dominus.WhatsappLog.WhatsappLog, Guid> _repository;

    public WhatsappLogAppService(
        IRepository<Dominus.WhatsappLog.WhatsappLog, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single WhatsappLog by Id
    /// </summary>
    public virtual async Task<WhatsappLogDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.WhatsappLog.WhatsappLog, WhatsappLogDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of WhatsappLogs
    /// </summary>
    public virtual async Task<PagedResultDto<WhatsappLogDto>> GetListAsync(WhatsappLogGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.WhatsappLog.WhatsappLog>, List<WhatsappLogDto>>(entities);

        return new PagedResultDto<WhatsappLogDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new WhatsappLog
    /// </summary>
    [Authorize(WhatsappLogPermissions.Create)]
    public virtual async Task<WhatsappLogDto> CreateAsync(CreateUpdateWhatsappLogDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateWhatsappLogDto, Dominus.WhatsappLog.WhatsappLog>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.WhatsappLog.WhatsappLog, WhatsappLogDto>(entity);
    }

    /// <summary>
    /// Updates an existing WhatsappLog
    /// </summary>
    [Authorize(WhatsappLogPermissions.Update)]
    public virtual async Task<WhatsappLogDto> UpdateAsync(Guid id, CreateUpdateWhatsappLogDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.WhatsappLog.WhatsappLog), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.WhatsappLog.WhatsappLog, WhatsappLogDto>(entity);
    }

    /// <summary>
    /// Deletes a WhatsappLog
    /// </summary>
    [Authorize(WhatsappLogPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetWhatsappLogLookupAsync()
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
    protected virtual IQueryable<Dominus.WhatsappLog.WhatsappLog> ApplyFilters(IQueryable<Dominus.WhatsappLog.WhatsappLog> queryable, WhatsappLogGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
