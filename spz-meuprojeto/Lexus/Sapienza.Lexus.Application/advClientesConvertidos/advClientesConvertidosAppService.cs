using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advClientesConvertidos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advClientesConvertidos;

/// <summary>
/// Application service for advClientesConvertidos entity
/// </summary>
[Authorize(advClientesConvertidosPermissions.Default)]
public class advClientesConvertidosAppService :
    LexusAppService,
    IadvClientesConvertidosAppService
{
    private readonly IRepository<Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos, Guid> _repository;

    public advClientesConvertidosAppService(
        IRepository<Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advClientesConvertidos by Id
    /// </summary>
    public virtual async Task<advClientesConvertidosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos, advClientesConvertidosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advClientesConvertidoses
    /// </summary>
    public virtual async Task<PagedResultDto<advClientesConvertidosDto>> GetListAsync(advClientesConvertidosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos>, List<advClientesConvertidosDto>>(entities);

        return new PagedResultDto<advClientesConvertidosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advClientesConvertidos
    /// </summary>
    [Authorize(advClientesConvertidosPermissions.Create)]
    public virtual async Task<advClientesConvertidosDto> CreateAsync(CreateUpdateadvClientesConvertidosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvClientesConvertidosDto, Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos, advClientesConvertidosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advClientesConvertidos
    /// </summary>
    [Authorize(advClientesConvertidosPermissions.Update)]
    public virtual async Task<advClientesConvertidosDto> UpdateAsync(Guid id, CreateUpdateadvClientesConvertidosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos, advClientesConvertidosDto>(entity);
    }

    /// <summary>
    /// Deletes a advClientesConvertidos
    /// </summary>
    [Authorize(advClientesConvertidosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvClientesConvertidosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.data
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos> ApplyFilters(IQueryable<Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos> queryable, advClientesConvertidosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.data.Contains(input.Filter) || x.convertidoPor.Contains(input.Filter))
            .WhereIf(input.idRegistro != null, x => x.idRegistro == input.idRegistro)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(!input.data.IsNullOrWhiteSpace(), x => x.data.Contains(input.data))
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(!input.convertidoPor.IsNullOrWhiteSpace(), x => x.convertidoPor.Contains(input.convertidoPor))
            // ========== FK Filters ==========
            ;
    }
}
