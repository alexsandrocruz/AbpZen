using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProfissionaisEstados.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProfissionaisEstados;

/// <summary>
/// Application service for advProfissionaisEstados entity
/// </summary>
[Authorize(advProfissionaisEstadosPermissions.Default)]
public class advProfissionaisEstadosAppService :
    LexusAppService,
    IadvProfissionaisEstadosAppService
{
    private readonly IRepository<Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados, Guid> _repository;

    public advProfissionaisEstadosAppService(
        IRepository<Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProfissionaisEstados by Id
    /// </summary>
    public virtual async Task<advProfissionaisEstadosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados, advProfissionaisEstadosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProfissionaisEstadoses
    /// </summary>
    public virtual async Task<PagedResultDto<advProfissionaisEstadosDto>> GetListAsync(advProfissionaisEstadosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados>, List<advProfissionaisEstadosDto>>(entities);

        return new PagedResultDto<advProfissionaisEstadosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProfissionaisEstados
    /// </summary>
    [Authorize(advProfissionaisEstadosPermissions.Create)]
    public virtual async Task<advProfissionaisEstadosDto> CreateAsync(CreateUpdateadvProfissionaisEstadosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProfissionaisEstadosDto, Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados, advProfissionaisEstadosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProfissionaisEstados
    /// </summary>
    [Authorize(advProfissionaisEstadosPermissions.Update)]
    public virtual async Task<advProfissionaisEstadosDto> UpdateAsync(Guid id, CreateUpdateadvProfissionaisEstadosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados, advProfissionaisEstadosDto>(entity);
    }

    /// <summary>
    /// Deletes a advProfissionaisEstados
    /// </summary>
    [Authorize(advProfissionaisEstadosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProfissionaisEstadosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.estado
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados> ApplyFilters(IQueryable<Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados> queryable, advProfissionaisEstadosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.estado.Contains(input.Filter))
            .WhereIf(input.idProfissionalEstado != null, x => x.idProfissionalEstado == input.idProfissionalEstado)
            .WhereIf(input.idProfissional != null, x => x.idProfissional == input.idProfissional)
            .WhereIf(!input.estado.IsNullOrWhiteSpace(), x => x.estado.Contains(input.estado))
            // ========== FK Filters ==========
            ;
    }
}
