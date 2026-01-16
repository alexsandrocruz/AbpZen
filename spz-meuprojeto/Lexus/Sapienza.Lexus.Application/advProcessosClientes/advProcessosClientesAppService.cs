using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProcessosClientes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProcessosClientes;

/// <summary>
/// Application service for advProcessosClientes entity
/// </summary>
[Authorize(advProcessosClientesPermissions.Default)]
public class advProcessosClientesAppService :
    LexusAppService,
    IadvProcessosClientesAppService
{
    private readonly IRepository<Sapienza.Lexus.advProcessosClientes.advProcessosClientes, Guid> _repository;

    public advProcessosClientesAppService(
        IRepository<Sapienza.Lexus.advProcessosClientes.advProcessosClientes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProcessosClientes by Id
    /// </summary>
    public virtual async Task<advProcessosClientesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProcessosClientes.advProcessosClientes, advProcessosClientesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProcessosClienteses
    /// </summary>
    public virtual async Task<PagedResultDto<advProcessosClientesDto>> GetListAsync(advProcessosClientesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProcessosClientes.advProcessosClientes>, List<advProcessosClientesDto>>(entities);

        return new PagedResultDto<advProcessosClientesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProcessosClientes
    /// </summary>
    [Authorize(advProcessosClientesPermissions.Create)]
    public virtual async Task<advProcessosClientesDto> CreateAsync(CreateUpdateadvProcessosClientesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProcessosClientesDto, Sapienza.Lexus.advProcessosClientes.advProcessosClientes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessosClientes.advProcessosClientes, advProcessosClientesDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProcessosClientes
    /// </summary>
    [Authorize(advProcessosClientesPermissions.Update)]
    public virtual async Task<advProcessosClientesDto> UpdateAsync(Guid id, CreateUpdateadvProcessosClientesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProcessosClientes.advProcessosClientes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessosClientes.advProcessosClientes, advProcessosClientesDto>(entity);
    }

    /// <summary>
    /// Deletes a advProcessosClientes
    /// </summary>
    [Authorize(advProcessosClientesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosClientesLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Id.ToString()
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advProcessosClientes.advProcessosClientes> ApplyFilters(IQueryable<Sapienza.Lexus.advProcessosClientes.advProcessosClientes> queryable, advProcessosClientesGetListInput input)
    {
        return queryable
            .WhereIf(input.idProcessoCliente != null, x => x.idProcessoCliente == input.idProcessoCliente)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            // ========== FK Filters ==========
            ;
    }
}
