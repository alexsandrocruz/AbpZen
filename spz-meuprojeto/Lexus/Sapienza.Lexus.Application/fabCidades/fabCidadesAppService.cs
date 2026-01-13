using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabCidades.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabCidades;

/// <summary>
/// Application service for fabCidades entity
/// </summary>
[Authorize(fabCidadesPermissions.Default)]
public class fabCidadesAppService :
    LexusAppService,
    IfabCidadesAppService
{
    private readonly IRepository<Sapienza.Lexus.fabCidades.fabCidades, Guid> _repository;

    public fabCidadesAppService(
        IRepository<Sapienza.Lexus.fabCidades.fabCidades, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fabCidades by Id
    /// </summary>
    public virtual async Task<fabCidadesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabCidades.fabCidades, fabCidadesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabCidadeses
    /// </summary>
    public virtual async Task<PagedResultDto<fabCidadesDto>> GetListAsync(fabCidadesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabCidades.fabCidades>, List<fabCidadesDto>>(entities);

        return new PagedResultDto<fabCidadesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabCidades
    /// </summary>
    [Authorize(fabCidadesPermissions.Create)]
    public virtual async Task<fabCidadesDto> CreateAsync(CreateUpdatefabCidadesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabCidadesDto, Sapienza.Lexus.fabCidades.fabCidades>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabCidades.fabCidades, fabCidadesDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabCidades
    /// </summary>
    [Authorize(fabCidadesPermissions.Update)]
    public virtual async Task<fabCidadesDto> UpdateAsync(Guid id, CreateUpdatefabCidadesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabCidades.fabCidades), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabCidades.fabCidades, fabCidadesDto>(entity);
    }

    /// <summary>
    /// Deletes a fabCidades
    /// </summary>
    [Authorize(fabCidadesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabCidadesLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.descricao
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.fabCidades.fabCidades> ApplyFilters(IQueryable<Sapienza.Lexus.fabCidades.fabCidades> queryable, fabCidadesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.descricao.Contains(input.Filter) || x.codigoIBGE.Contains(input.Filter))
            .WhereIf(input.idCidade != null, x => x.idCidade == input.idCidade)
            .WhereIf(!input.descricao.IsNullOrWhiteSpace(), x => x.descricao.Contains(input.descricao))
            .WhereIf(!input.codigoIBGE.IsNullOrWhiteSpace(), x => x.codigoIBGE.Contains(input.codigoIBGE))
            .WhereIf(input.idEstado != null, x => x.idEstado == input.idEstado)
            // ========== FK Filters ==========
            ;
    }
}
