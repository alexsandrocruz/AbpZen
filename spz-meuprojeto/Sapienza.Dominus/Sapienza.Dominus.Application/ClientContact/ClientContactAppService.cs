using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.ClientContact.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ClientContact;

/// <summary>
/// Application service for ClientContact entity
/// </summary>
[Authorize(ClientContactPermissions.Default)]
public class ClientContactAppService :
    DominusAppService,
    IClientContactAppService
{
    private readonly IRepository<Dominus.ClientContact.ClientContact, Guid> _repository;
    private readonly IRepository<Dominus.Client.Client, Guid> _clientRepository;

    public ClientContactAppService(
        IRepository<Dominus.ClientContact.ClientContact, Guid> repository,
        IRepository<Dominus.Client.Client, Guid> clientRepository
    )
    {
        _repository = repository;
        _clientRepository = clientRepository;
    }

    /// <summary>
    /// Gets a single ClientContact by Id
    /// </summary>
    public virtual async Task<ClientContactDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.ClientContact.ClientContact, ClientContactDto>(entity);
        if (entity.ClientId != null)
        {
            var parent = await _clientRepository.FindAsync(entity.ClientId.Value);
            dto.ClientDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of ClientContacts
    /// </summary>
    public virtual async Task<PagedResultDto<ClientContactDto>> GetListAsync(ClientContactGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.ClientContact.ClientContact>, List<ClientContactDto>>(entities);
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

        return new PagedResultDto<ClientContactDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new ClientContact
    /// </summary>
    [Authorize(ClientContactPermissions.Create)]
    public virtual async Task<ClientContactDto> CreateAsync(CreateUpdateClientContactDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateClientContactDto, Dominus.ClientContact.ClientContact>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ClientContact.ClientContact, ClientContactDto>(entity);
    }

    /// <summary>
    /// Updates an existing ClientContact
    /// </summary>
    [Authorize(ClientContactPermissions.Update)]
    public virtual async Task<ClientContactDto> UpdateAsync(Guid id, CreateUpdateClientContactDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.ClientContact.ClientContact), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ClientContact.ClientContact, ClientContactDto>(entity);
    }

    /// <summary>
    /// Deletes a ClientContact
    /// </summary>
    [Authorize(ClientContactPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetClientContactLookupAsync()
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
    protected virtual IQueryable<Dominus.ClientContact.ClientContact> ApplyFilters(IQueryable<Dominus.ClientContact.ClientContact> queryable, ClientContactGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            ;
    }
}
