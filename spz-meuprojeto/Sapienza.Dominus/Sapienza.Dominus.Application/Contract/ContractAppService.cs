using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Contract.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Contract;

/// <summary>
/// Application service for Contract entity
/// </summary>
[Authorize(ContractPermissions.Default)]
public class ContractAppService :
    DominusAppService,
    IContractAppService
{
    private readonly IRepository<Dominus.Contract.Contract, Guid> _repository;
    private readonly IRepository<Dominus.Client.Client, Guid> _clientRepository;

    public ContractAppService(
        IRepository<Dominus.Contract.Contract, Guid> repository,
        IRepository<Dominus.Client.Client, Guid> clientRepository
    )
    {
        _repository = repository;
        _clientRepository = clientRepository;
    }

    /// <summary>
    /// Gets a single Contract by Id
    /// </summary>
    public virtual async Task<ContractDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Contract.Contract, ContractDto>(entity);
        if (entity.ClientId != null)
        {
            var parent = await _clientRepository.FindAsync(entity.ClientId.Value);
            dto.ClientDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Contracts
    /// </summary>
    public virtual async Task<PagedResultDto<ContractDto>> GetListAsync(ContractGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Contract.Contract>, List<ContractDto>>(entities);
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

        return new PagedResultDto<ContractDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Contract
    /// </summary>
    [Authorize(ContractPermissions.Create)]
    public virtual async Task<ContractDto> CreateAsync(CreateUpdateContractDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateContractDto, Dominus.Contract.Contract>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Contract.Contract, ContractDto>(entity);
    }

    /// <summary>
    /// Updates an existing Contract
    /// </summary>
    [Authorize(ContractPermissions.Update)]
    public virtual async Task<ContractDto> UpdateAsync(Guid id, CreateUpdateContractDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Contract.Contract), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Contract.Contract, ContractDto>(entity);
    }

    /// <summary>
    /// Deletes a Contract
    /// </summary>
    [Authorize(ContractPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetContractLookupAsync()
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
    protected virtual IQueryable<Dominus.Contract.Contract> ApplyFilters(IQueryable<Dominus.Contract.Contract> queryable, ContractGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            ;
    }
}
