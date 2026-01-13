using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCliTiposHistoricos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliTiposHistoricos;

/// <summary>
/// Application service for advCliTiposHistoricos entity
/// </summary>
[Authorize(advCliTiposHistoricosPermissions.Default)]
public class advCliTiposHistoricosAppService :
    LexusAppService,
    IadvCliTiposHistoricosAppService
{
    private readonly IRepository<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos, Guid> _repository;

    public advCliTiposHistoricosAppService(
        IRepository<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advCliTiposHistoricos by Id
    /// </summary>
    public virtual async Task<advCliTiposHistoricosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos, advCliTiposHistoricosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCliTiposHistoricoses
    /// </summary>
    public virtual async Task<PagedResultDto<advCliTiposHistoricosDto>> GetListAsync(advCliTiposHistoricosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos>, List<advCliTiposHistoricosDto>>(entities);

        return new PagedResultDto<advCliTiposHistoricosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCliTiposHistoricos
    /// </summary>
    [Authorize(advCliTiposHistoricosPermissions.Create)]
    public virtual async Task<advCliTiposHistoricosDto> CreateAsync(CreateUpdateadvCliTiposHistoricosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCliTiposHistoricosDto, Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos, advCliTiposHistoricosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCliTiposHistoricos
    /// </summary>
    [Authorize(advCliTiposHistoricosPermissions.Update)]
    public virtual async Task<advCliTiposHistoricosDto> UpdateAsync(Guid id, CreateUpdateadvCliTiposHistoricosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos, advCliTiposHistoricosDto>(entity);
    }

    /// <summary>
    /// Deletes a advCliTiposHistoricos
    /// </summary>
    [Authorize(advCliTiposHistoricosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCliTiposHistoricosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.titulo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos> ApplyFilters(IQueryable<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos> queryable, advCliTiposHistoricosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idTipoHistorico != null, x => x.idTipoHistorico == input.idTipoHistorico)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
