using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Client.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Client;

/// <summary>
/// Application service for Client entity
/// </summary>
[Authorize(ClientPermissions.Default)]
public class ClientAppService :
    DominusAppService,
    IClientAppService
{
    private readonly IRepository<Dominus.Client.Client, Guid> _repository;

    public ClientAppService(
        IRepository<Dominus.Client.Client, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Client by Id
    /// </summary>
    public virtual async Task<ClientDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Client.Client, ClientDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Clients
    /// </summary>
    public virtual async Task<PagedResultDto<ClientDto>> GetListAsync(ClientGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Client.Client>, List<ClientDto>>(entities);

        return new PagedResultDto<ClientDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Client
    /// </summary>
    [Authorize(ClientPermissions.Create)]
    public virtual async Task<ClientDto> CreateAsync(CreateUpdateClientDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateClientDto, Dominus.Client.Client>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Client.Client, ClientDto>(entity);
    }

    /// <summary>
    /// Updates an existing Client
    /// </summary>
    [Authorize(ClientPermissions.Update)]
    public virtual async Task<ClientDto> UpdateAsync(Guid id, CreateUpdateClientDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Client.Client), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Client.Client, ClientDto>(entity);
    }

    /// <summary>
    /// Deletes a Client
    /// </summary>
    [Authorize(ClientPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetClientLookupAsync()
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
    protected virtual IQueryable<Dominus.Client.Client> ApplyFilters(IQueryable<Dominus.Client.Client> queryable, ClientGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
