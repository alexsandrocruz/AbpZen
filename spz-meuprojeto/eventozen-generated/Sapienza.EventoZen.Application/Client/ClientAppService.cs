using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.EventoZen.Permissions;
using Sapienza.EventoZen.Client.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.Client;

/// <summary>
/// Application service for Client entity
/// </summary>
[Authorize(ClientPermissions.Default)]
public class ClientAppService :
    EventoZenAppService,
    IClientAppService
{
    private readonly IRepository<Sapienza.EventoZen.Client.Client, Guid> _repository;

    public ClientAppService(
        IRepository<Sapienza.EventoZen.Client.Client, Guid> repository
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
        var dto = ObjectMapper.Map<Sapienza.EventoZen.Client.Client, ClientDto>(entity);

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
        var dtoList = ObjectMapper.Map<List<Sapienza.EventoZen.Client.Client>, List<ClientDto>>(entities);

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
        var entity = ObjectMapper.Map<CreateUpdateClientDto, Sapienza.EventoZen.Client.Client>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.Client.Client, ClientDto>(entity);
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
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.EventoZen.Client.Client), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.EventoZen.Client.Client, ClientDto>(entity);
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
    protected virtual IQueryable<Sapienza.EventoZen.Client.Client> ApplyFilters(IQueryable<Sapienza.EventoZen.Client.Client> queryable, ClientGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.Name.Contains(input.Filter) || x.Document.Contains(input.Filter) || x.Email.Contains(input.Filter) || x.Phone.Contains(input.Filter) || x.Address.Contains(input.Filter) || x.City.Contains(input.Filter) || x.State.Contains(input.Filter) || x.Notes.Contains(input.Filter))
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            .WhereIf(input.Type != null, x => x.Type == input.Type)
            .WhereIf(!input.Document.IsNullOrWhiteSpace(), x => x.Document.Contains(input.Document))
            .WhereIf(!input.Email.IsNullOrWhiteSpace(), x => x.Email.Contains(input.Email))
            .WhereIf(!input.Phone.IsNullOrWhiteSpace(), x => x.Phone.Contains(input.Phone))
            .WhereIf(!input.Address.IsNullOrWhiteSpace(), x => x.Address.Contains(input.Address))
            .WhereIf(!input.City.IsNullOrWhiteSpace(), x => x.City.Contains(input.City))
            .WhereIf(!input.State.IsNullOrWhiteSpace(), x => x.State.Contains(input.State))
            .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes.Contains(input.Notes))
            .WhereIf(input.IsActive != null, x => x.IsActive == input.IsActive)
            .WhereIf(input.LeadStatus != null, x => x.LeadStatus == input.LeadStatus)
            .WhereIf(input.FirstContactDate != null, x => x.FirstContactDate == input.FirstContactDate)
            .WhereIf(input.LastContactDate != null, x => x.LastContactDate == input.LastContactDate)
            // ========== FK Filters ==========
            ;
    }
}
