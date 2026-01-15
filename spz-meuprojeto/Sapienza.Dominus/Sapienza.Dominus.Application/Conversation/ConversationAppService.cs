using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Conversation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Conversation;

/// <summary>
/// Application service for Conversation entity
/// </summary>
[Authorize(ConversationPermissions.Default)]
public class ConversationAppService :
    DominusAppService,
    IConversationAppService
{
    private readonly IRepository<Dominus.Conversation.Conversation, Guid> _repository;
    private readonly IRepository<Dominus.Client.Client, Guid> _clientRepository;

    public ConversationAppService(
        IRepository<Dominus.Conversation.Conversation, Guid> repository,
        IRepository<Dominus.Client.Client, Guid> clientRepository
    )
    {
        _repository = repository;
        _clientRepository = clientRepository;
    }

    /// <summary>
    /// Gets a single Conversation by Id
    /// </summary>
    public virtual async Task<ConversationDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Conversation.Conversation, ConversationDto>(entity);
        if (entity.ClientId != null)
        {
            var parent = await _clientRepository.FindAsync(entity.ClientId.Value);
            dto.ClientDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Conversations
    /// </summary>
    public virtual async Task<PagedResultDto<ConversationDto>> GetListAsync(ConversationGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Conversation.Conversation>, List<ConversationDto>>(entities);
        var clientIds = entities
            .Where(x => x.ClientId != null)
            .Select(x => x.ClientId.Value)
            .Distinct()
            .ToList();

        if (clientIds.Any())
        {
            var parents = await _clientRepository.GetListAsync(x => clientIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.ClientId != null))
            {
                if (parentMap.TryGetValue(dto.ClientId.Value, out var displayName))
                {
                    dto.ClientDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<ConversationDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Conversation
    /// </summary>
    [Authorize(ConversationPermissions.Create)]
    public virtual async Task<ConversationDto> CreateAsync(CreateUpdateConversationDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateConversationDto, Dominus.Conversation.Conversation>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Conversation.Conversation, ConversationDto>(entity);
    }

    /// <summary>
    /// Updates an existing Conversation
    /// </summary>
    [Authorize(ConversationPermissions.Update)]
    public virtual async Task<ConversationDto> UpdateAsync(Guid id, CreateUpdateConversationDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Conversation.Conversation), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Conversation.Conversation, ConversationDto>(entity);
    }

    /// <summary>
    /// Deletes a Conversation
    /// </summary>
    [Authorize(ConversationPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetConversationLookupAsync()
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
    protected virtual IQueryable<Dominus.Conversation.Conversation> ApplyFilters(IQueryable<Dominus.Conversation.Conversation> queryable, ConversationGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            ;
    }
}
