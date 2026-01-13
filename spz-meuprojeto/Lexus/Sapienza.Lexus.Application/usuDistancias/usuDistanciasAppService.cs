using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.usuDistancias.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.usuDistancias;

/// <summary>
/// Application service for usuDistancias entity
/// </summary>
[Authorize(usuDistanciasPermissions.Default)]
public class usuDistanciasAppService :
    LexusAppService,
    IusuDistanciasAppService
{
    private readonly IRepository<Sapienza.Lexus.usuDistancias.usuDistancias, Guid> _repository;

    public usuDistanciasAppService(
        IRepository<Sapienza.Lexus.usuDistancias.usuDistancias, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single usuDistancias by Id
    /// </summary>
    public virtual async Task<usuDistanciasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.usuDistancias.usuDistancias, usuDistanciasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of usuDistanciases
    /// </summary>
    public virtual async Task<PagedResultDto<usuDistanciasDto>> GetListAsync(usuDistanciasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.usuDistancias.usuDistancias>, List<usuDistanciasDto>>(entities);

        return new PagedResultDto<usuDistanciasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new usuDistancias
    /// </summary>
    [Authorize(usuDistanciasPermissions.Create)]
    public virtual async Task<usuDistanciasDto> CreateAsync(CreateUpdateusuDistanciasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateusuDistanciasDto, Sapienza.Lexus.usuDistancias.usuDistancias>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuDistancias.usuDistancias, usuDistanciasDto>(entity);
    }

    /// <summary>
    /// Updates an existing usuDistancias
    /// </summary>
    [Authorize(usuDistanciasPermissions.Update)]
    public virtual async Task<usuDistanciasDto> UpdateAsync(Guid id, CreateUpdateusuDistanciasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.usuDistancias.usuDistancias), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuDistancias.usuDistancias, usuDistanciasDto>(entity);
    }

    /// <summary>
    /// Deletes a usuDistancias
    /// </summary>
    [Authorize(usuDistanciasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetusuDistanciasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.usuDistancias.usuDistancias> ApplyFilters(IQueryable<Sapienza.Lexus.usuDistancias.usuDistancias> queryable, usuDistanciasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.estado.Contains(input.Filter) || x.cidade.Contains(input.Filter))
            .WhereIf(input.idDistancia != null, x => x.idDistancia == input.idDistancia)
            .WhereIf(input.idUsuario != null, x => x.idUsuario == input.idUsuario)
            .WhereIf(!input.estado.IsNullOrWhiteSpace(), x => x.estado.Contains(input.estado))
            .WhereIf(!input.cidade.IsNullOrWhiteSpace(), x => x.cidade.Contains(input.cidade))
            .WhereIf(input.km != null, x => x.km == input.km)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            // ========== FK Filters ==========
            ;
    }
}
