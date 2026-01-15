using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.ChatMessage.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ChatMessage;

/// <summary>
/// Application service for ChatMessage entity
/// </summary>
[Authorize(ChatMessagePermissions.Default)]
public class ChatMessageAppService :
    DominusAppService,
    IChatMessageAppService
{
    private readonly IRepository<Dominus.ChatMessage.ChatMessage, Guid> _repository;
    private readonly IRepository<Dominus.Conversation.Conversation, Guid> _conversationRepository;

    public ChatMessageAppService(
        IRepository<Dominus.ChatMessage.ChatMessage, Guid> repository,
        IRepository<Dominus.Conversation.Conversation, Guid> conversationRepository
    )
    {
        _repository = repository;
        _conversationRepository = conversationRepository;
    }

    /// <summary>
    /// Gets a single ChatMessage by Id
    /// </summary>
    public virtual async Task<ChatMessageDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.ChatMessage.ChatMessage, ChatMessageDto>(entity);
        if (entity.ConversationId != null)
        {
            var parent = await _conversationRepository.FindAsync(entity.ConversationId.Value);
            dto.ConversationDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of ChatMessages
    /// </summary>
    public virtual async Task<PagedResultDto<ChatMessageDto>> GetListAsync(ChatMessageGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.ChatMessage.ChatMessage>, List<ChatMessageDto>>(entities);
        var conversationIds = entities
            .Where(x => x.ConversationId != null)
            .Select(x => x.ConversationId.Value)
            .Distinct()
            .ToList();

        if (conversationIds.Any())
        {
            var parents = await _conversationRepository.GetListAsync(x => conversationIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.ConversationId != null))
            {
                if (parentMap.TryGetValue(dto.ConversationId.Value, out var displayName))
                {
                    dto.ConversationDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<ChatMessageDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new ChatMessage
    /// </summary>
    [Authorize(ChatMessagePermissions.Create)]
    public virtual async Task<ChatMessageDto> CreateAsync(CreateUpdateChatMessageDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateChatMessageDto, Dominus.ChatMessage.ChatMessage>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ChatMessage.ChatMessage, ChatMessageDto>(entity);
    }

    /// <summary>
    /// Updates an existing ChatMessage
    /// </summary>
    [Authorize(ChatMessagePermissions.Update)]
    public virtual async Task<ChatMessageDto> UpdateAsync(Guid id, CreateUpdateChatMessageDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.ChatMessage.ChatMessage), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ChatMessage.ChatMessage, ChatMessageDto>(entity);
    }

    /// <summary>
    /// Deletes a ChatMessage
    /// </summary>
    [Authorize(ChatMessagePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetChatMessageLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Content
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.ChatMessage.ChatMessage> ApplyFilters(IQueryable<Dominus.ChatMessage.ChatMessage> queryable, ChatMessageGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ConversationId != null, x => x.ConversationId == input.ConversationId)
            ;
    }
}
