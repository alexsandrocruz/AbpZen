using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabDatasEFeriados.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabDatasEFeriados;

/// <summary>
/// Application service for fabDatasEFeriados entity
/// </summary>
[Authorize(fabDatasEFeriadosPermissions.Default)]
public class fabDatasEFeriadosAppService :
    LexusAppService,
    IfabDatasEFeriadosAppService
{
    private readonly IRepository<Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados, Guid> _repository;

    public fabDatasEFeriadosAppService(
        IRepository<Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fabDatasEFeriados by Id
    /// </summary>
    public virtual async Task<fabDatasEFeriadosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados, fabDatasEFeriadosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabDatasEFeriadoses
    /// </summary>
    public virtual async Task<PagedResultDto<fabDatasEFeriadosDto>> GetListAsync(fabDatasEFeriadosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados>, List<fabDatasEFeriadosDto>>(entities);

        return new PagedResultDto<fabDatasEFeriadosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabDatasEFeriados
    /// </summary>
    [Authorize(fabDatasEFeriadosPermissions.Create)]
    public virtual async Task<fabDatasEFeriadosDto> CreateAsync(CreateUpdatefabDatasEFeriadosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabDatasEFeriadosDto, Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados, fabDatasEFeriadosDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabDatasEFeriados
    /// </summary>
    [Authorize(fabDatasEFeriadosPermissions.Update)]
    public virtual async Task<fabDatasEFeriadosDto> UpdateAsync(Guid id, CreateUpdatefabDatasEFeriadosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados, fabDatasEFeriadosDto>(entity);
    }

    /// <summary>
    /// Deletes a fabDatasEFeriados
    /// </summary>
    [Authorize(fabDatasEFeriadosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabDatasEFeriadosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados> ApplyFilters(IQueryable<Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados> queryable, fabDatasEFeriadosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.data.Contains(input.Filter))
            .WhereIf(input.idData != null, x => x.idData == input.idData)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(!input.data.IsNullOrWhiteSpace(), x => x.data.Contains(input.data))
            .WhereIf(input.feriado != null, x => x.feriado == input.feriado)
            .WhereIf(input.fixo != null, x => x.fixo == input.fixo)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
