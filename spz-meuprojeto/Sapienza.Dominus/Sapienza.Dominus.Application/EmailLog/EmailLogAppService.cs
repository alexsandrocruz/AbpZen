using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.EmailLog.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.EmailLog;

/// <summary>
/// Application service for EmailLog entity
/// </summary>
[Authorize(EmailLogPermissions.Default)]
public class EmailLogAppService :
    DominusAppService,
    IEmailLogAppService
{
    private readonly IRepository<Dominus.EmailLog.EmailLog, Guid> _repository;

    public EmailLogAppService(
        IRepository<Dominus.EmailLog.EmailLog, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single EmailLog by Id
    /// </summary>
    public virtual async Task<EmailLogDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.EmailLog.EmailLog, EmailLogDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of EmailLogs
    /// </summary>
    public virtual async Task<PagedResultDto<EmailLogDto>> GetListAsync(EmailLogGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.EmailLog.EmailLog>, List<EmailLogDto>>(entities);

        return new PagedResultDto<EmailLogDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new EmailLog
    /// </summary>
    [Authorize(EmailLogPermissions.Create)]
    public virtual async Task<EmailLogDto> CreateAsync(CreateUpdateEmailLogDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateEmailLogDto, Dominus.EmailLog.EmailLog>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.EmailLog.EmailLog, EmailLogDto>(entity);
    }

    /// <summary>
    /// Updates an existing EmailLog
    /// </summary>
    [Authorize(EmailLogPermissions.Update)]
    public virtual async Task<EmailLogDto> UpdateAsync(Guid id, CreateUpdateEmailLogDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.EmailLog.EmailLog), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.EmailLog.EmailLog, EmailLogDto>(entity);
    }

    /// <summary>
    /// Deletes a EmailLog
    /// </summary>
    [Authorize(EmailLogPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetEmailLogLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.ToEmail
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.EmailLog.EmailLog> ApplyFilters(IQueryable<Dominus.EmailLog.EmailLog> queryable, EmailLogGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
