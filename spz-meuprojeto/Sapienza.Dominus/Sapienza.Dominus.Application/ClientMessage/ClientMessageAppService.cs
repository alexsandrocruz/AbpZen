using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.ClientMessage.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ClientMessage;

/// <summary>
/// Application service for ClientMessage entity
/// </summary>
[Authorize(ClientMessagePermissions.Default)]
public class ClientMessageAppService :
    DominusAppService,
    IClientMessageAppService
{
    private readonly IRepository<Dominus.ClientMessage.ClientMessage, Guid> _repository;
    private readonly IRepository<Dominus.Client.Client, Guid> _clientRepository;

    public ClientMessageAppService(
        IRepository<Dominus.ClientMessage.ClientMessage, Guid> repository,
        IRepository<Dominus.Client.Client, Guid> clientRepository
    )
    {
        _repository = repository;
        _clientRepository = clientRepository;
    }

    /// <summary>
    /// Gets a single ClientMessage by Id
    /// </summary>
    public virtual async Task<ClientMessageDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.ClientMessage.ClientMessage, ClientMessageDto>(entity);
        if (entity.ClientId != null)
        {
            var parent = await _clientRepository.FindAsync(entity.ClientId.Value);
            dto.ClientDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of ClientMessages
    /// </summary>
    public virtual async Task<PagedResultDto<ClientMessageDto>> GetListAsync(ClientMessageGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.ClientMessage.ClientMessage>, List<ClientMessageDto>>(entities);
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

        return new PagedResultDto<ClientMessageDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new ClientMessage
    /// </summary>
    [Authorize(ClientMessagePermissions.Create)]
    public virtual async Task<ClientMessageDto> CreateAsync(CreateUpdateClientMessageDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateClientMessageDto, Dominus.ClientMessage.ClientMessage>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ClientMessage.ClientMessage, ClientMessageDto>(entity);
    }

    /// <summary>
    /// Updates an existing ClientMessage
    /// </summary>
    [Authorize(ClientMessagePermissions.Update)]
    public virtual async Task<ClientMessageDto> UpdateAsync(Guid id, CreateUpdateClientMessageDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.ClientMessage.ClientMessage), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ClientMessage.ClientMessage, ClientMessageDto>(entity);
    }

    /// <summary>
    /// Deletes a ClientMessage
    /// </summary>
    [Authorize(ClientMessagePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetClientMessageLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Channel
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.ClientMessage.ClientMessage> ApplyFilters(IQueryable<Dominus.ClientMessage.ClientMessage> queryable, ClientMessageGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            ;
    }
}
