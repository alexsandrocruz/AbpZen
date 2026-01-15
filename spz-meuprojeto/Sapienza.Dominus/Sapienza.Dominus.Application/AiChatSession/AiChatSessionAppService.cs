using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.AiChatSession.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.AiChatSession;

/// <summary>
/// Application service for AiChatSession entity
/// </summary>
[Authorize(AiChatSessionPermissions.Default)]
public class AiChatSessionAppService :
    DominusAppService,
    IAiChatSessionAppService
{
    private readonly IRepository<Dominus.AiChatSession.AiChatSession, Guid> _repository;

    public AiChatSessionAppService(
        IRepository<Dominus.AiChatSession.AiChatSession, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single AiChatSession by Id
    /// </summary>
    public virtual async Task<AiChatSessionDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.AiChatSession.AiChatSession, AiChatSessionDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of AiChatSessions
    /// </summary>
    public virtual async Task<PagedResultDto<AiChatSessionDto>> GetListAsync(AiChatSessionGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.AiChatSession.AiChatSession>, List<AiChatSessionDto>>(entities);

        return new PagedResultDto<AiChatSessionDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new AiChatSession
    /// </summary>
    [Authorize(AiChatSessionPermissions.Create)]
    public virtual async Task<AiChatSessionDto> CreateAsync(CreateUpdateAiChatSessionDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateAiChatSessionDto, Dominus.AiChatSession.AiChatSession>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.AiChatSession.AiChatSession, AiChatSessionDto>(entity);
    }

    /// <summary>
    /// Updates an existing AiChatSession
    /// </summary>
    [Authorize(AiChatSessionPermissions.Update)]
    public virtual async Task<AiChatSessionDto> UpdateAsync(Guid id, CreateUpdateAiChatSessionDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.AiChatSession.AiChatSession), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.AiChatSession.AiChatSession, AiChatSessionDto>(entity);
    }

    /// <summary>
    /// Deletes a AiChatSession
    /// </summary>
    [Authorize(AiChatSessionPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetAiChatSessionLookupAsync()
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
    protected virtual IQueryable<Dominus.AiChatSession.AiChatSession> ApplyFilters(IQueryable<Dominus.AiChatSession.AiChatSession> queryable, AiChatSessionGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
