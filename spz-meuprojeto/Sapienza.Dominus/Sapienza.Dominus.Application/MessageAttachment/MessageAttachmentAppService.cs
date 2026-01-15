using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.MessageAttachment.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.MessageAttachment;

/// <summary>
/// Application service for MessageAttachment entity
/// </summary>
[Authorize(MessageAttachmentPermissions.Default)]
public class MessageAttachmentAppService :
    DominusAppService,
    IMessageAttachmentAppService
{
    private readonly IRepository<Dominus.MessageAttachment.MessageAttachment, Guid> _repository;
    private readonly IRepository<Dominus.ChatMessage.ChatMessage, Guid> _chatMessageRepository;

    public MessageAttachmentAppService(
        IRepository<Dominus.MessageAttachment.MessageAttachment, Guid> repository,
        IRepository<Dominus.ChatMessage.ChatMessage, Guid> chatMessageRepository
    )
    {
        _repository = repository;
        _chatMessageRepository = chatMessageRepository;
    }

    /// <summary>
    /// Gets a single MessageAttachment by Id
    /// </summary>
    public virtual async Task<MessageAttachmentDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.MessageAttachment.MessageAttachment, MessageAttachmentDto>(entity);
        if (entity.ChatMessageId != null)
        {
            var parent = await _chatMessageRepository.FindAsync(entity.ChatMessageId.Value);
            dto.ChatMessageDisplayName = parent?.Content;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of MessageAttachments
    /// </summary>
    public virtual async Task<PagedResultDto<MessageAttachmentDto>> GetListAsync(MessageAttachmentGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.MessageAttachment.MessageAttachment>, List<MessageAttachmentDto>>(entities);
        var chatMessageIds = entities
            .Where(x => x.ChatMessageId != null)
            .Select(x => x.ChatMessageId.Value)
            .Distinct()
            .ToList();

        if (chatMessageIds.Any())
        {
            var parents = await _chatMessageRepository.GetListAsync(x => chatMessageIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Content);

            foreach (var dto in dtoList.Where(x => x.ChatMessageId != null))
            {
                if (parentMap.TryGetValue(dto.ChatMessageId.Value, out var displayName))
                {
                    dto.ChatMessageDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<MessageAttachmentDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new MessageAttachment
    /// </summary>
    [Authorize(MessageAttachmentPermissions.Create)]
    public virtual async Task<MessageAttachmentDto> CreateAsync(CreateUpdateMessageAttachmentDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateMessageAttachmentDto, Dominus.MessageAttachment.MessageAttachment>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.MessageAttachment.MessageAttachment, MessageAttachmentDto>(entity);
    }

    /// <summary>
    /// Updates an existing MessageAttachment
    /// </summary>
    [Authorize(MessageAttachmentPermissions.Update)]
    public virtual async Task<MessageAttachmentDto> UpdateAsync(Guid id, CreateUpdateMessageAttachmentDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.MessageAttachment.MessageAttachment), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.MessageAttachment.MessageAttachment, MessageAttachmentDto>(entity);
    }

    /// <summary>
    /// Deletes a MessageAttachment
    /// </summary>
    [Authorize(MessageAttachmentPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetMessageAttachmentLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.FileName
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.MessageAttachment.MessageAttachment> ApplyFilters(IQueryable<Dominus.MessageAttachment.MessageAttachment> queryable, MessageAttachmentGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ChatMessageId != null, x => x.ChatMessageId == input.ChatMessageId)
            ;
    }
}
