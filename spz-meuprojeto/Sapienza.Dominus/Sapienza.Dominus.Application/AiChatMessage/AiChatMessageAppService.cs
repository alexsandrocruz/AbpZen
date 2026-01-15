using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.AiChatMessage.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.AiChatMessage;

/// <summary>
/// Application service for AiChatMessage entity
/// </summary>
[Authorize(AiChatMessagePermissions.Default)]
public class AiChatMessageAppService :
    DominusAppService,
    IAiChatMessageAppService
{
    private readonly IRepository<Dominus.AiChatMessage.AiChatMessage, Guid> _repository;
    private readonly IRepository<Dominus.AiChatSession.AiChatSession, Guid> _aiChatSessionRepository;

    public AiChatMessageAppService(
        IRepository<Dominus.AiChatMessage.AiChatMessage, Guid> repository,
        IRepository<Dominus.AiChatSession.AiChatSession, Guid> aiChatSessionRepository
    )
    {
        _repository = repository;
        _aiChatSessionRepository = aiChatSessionRepository;
    }

    /// <summary>
    /// Gets a single AiChatMessage by Id
    /// </summary>
    public virtual async Task<AiChatMessageDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.AiChatMessage.AiChatMessage, AiChatMessageDto>(entity);
        if (entity.AiChatSessionId != null)
        {
            var parent = await _aiChatSessionRepository.FindAsync(entity.AiChatSessionId.Value);
            dto.AiChatSessionDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of AiChatMessages
    /// </summary>
    public virtual async Task<PagedResultDto<AiChatMessageDto>> GetListAsync(AiChatMessageGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.AiChatMessage.AiChatMessage>, List<AiChatMessageDto>>(entities);
        var aiChatSessionIds = entities
            .Where(x => x.AiChatSessionId != null)
            .Select(x => x.AiChatSessionId.Value)
            .Distinct()
            .ToList();

        if (aiChatSessionIds.Any())
        {
            var parents = await _aiChatSessionRepository.GetListAsync(x => aiChatSessionIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Title);

            foreach (var dto in dtoList.Where(x => x.AiChatSessionId != null))
            {
                if (parentMap.TryGetValue(dto.AiChatSessionId.Value, out var displayName))
                {
                    dto.AiChatSessionDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<AiChatMessageDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new AiChatMessage
    /// </summary>
    [Authorize(AiChatMessagePermissions.Create)]
    public virtual async Task<AiChatMessageDto> CreateAsync(CreateUpdateAiChatMessageDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateAiChatMessageDto, Dominus.AiChatMessage.AiChatMessage>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.AiChatMessage.AiChatMessage, AiChatMessageDto>(entity);
    }

    /// <summary>
    /// Updates an existing AiChatMessage
    /// </summary>
    [Authorize(AiChatMessagePermissions.Update)]
    public virtual async Task<AiChatMessageDto> UpdateAsync(Guid id, CreateUpdateAiChatMessageDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.AiChatMessage.AiChatMessage), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.AiChatMessage.AiChatMessage, AiChatMessageDto>(entity);
    }

    /// <summary>
    /// Deletes a AiChatMessage
    /// </summary>
    [Authorize(AiChatMessagePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetAiChatMessageLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Role
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.AiChatMessage.AiChatMessage> ApplyFilters(IQueryable<Dominus.AiChatMessage.AiChatMessage> queryable, AiChatMessageGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.AiChatSessionId != null, x => x.AiChatSessionId == input.AiChatSessionId)
            ;
    }
}
