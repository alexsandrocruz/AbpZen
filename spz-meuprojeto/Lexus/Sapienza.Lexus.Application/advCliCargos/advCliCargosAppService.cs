using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCliCargos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliCargos;

/// <summary>
/// Application service for advCliCargos entity
/// </summary>
[Authorize(advCliCargosPermissions.Default)]
public class advCliCargosAppService :
    LexusAppService,
    IadvCliCargosAppService
{
    private readonly IRepository<Sapienza.Lexus.advCliCargos.advCliCargos, Guid> _repository;

    public advCliCargosAppService(
        IRepository<Sapienza.Lexus.advCliCargos.advCliCargos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advCliCargos by Id
    /// </summary>
    public virtual async Task<advCliCargosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCliCargos.advCliCargos, advCliCargosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCliCargoses
    /// </summary>
    public virtual async Task<PagedResultDto<advCliCargosDto>> GetListAsync(advCliCargosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCliCargos.advCliCargos>, List<advCliCargosDto>>(entities);

        return new PagedResultDto<advCliCargosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCliCargos
    /// </summary>
    [Authorize(advCliCargosPermissions.Create)]
    public virtual async Task<advCliCargosDto> CreateAsync(CreateUpdateadvCliCargosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCliCargosDto, Sapienza.Lexus.advCliCargos.advCliCargos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliCargos.advCliCargos, advCliCargosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCliCargos
    /// </summary>
    [Authorize(advCliCargosPermissions.Update)]
    public virtual async Task<advCliCargosDto> UpdateAsync(Guid id, CreateUpdateadvCliCargosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCliCargos.advCliCargos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliCargos.advCliCargos, advCliCargosDto>(entity);
    }

    /// <summary>
    /// Deletes a advCliCargos
    /// </summary>
    [Authorize(advCliCargosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCliCargosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advCliCargos.advCliCargos> ApplyFilters(IQueryable<Sapienza.Lexus.advCliCargos.advCliCargos> queryable, advCliCargosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idCargo != null, x => x.idCargo == input.idCargo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
