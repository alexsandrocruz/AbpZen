using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.usuCargos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.usuCargos;

/// <summary>
/// Application service for usuCargos entity
/// </summary>
[Authorize(usuCargosPermissions.Default)]
public class usuCargosAppService :
    LexusAppService,
    IusuCargosAppService
{
    private readonly IRepository<Sapienza.Lexus.usuCargos.usuCargos, Guid> _repository;

    public usuCargosAppService(
        IRepository<Sapienza.Lexus.usuCargos.usuCargos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single usuCargos by Id
    /// </summary>
    public virtual async Task<usuCargosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.usuCargos.usuCargos, usuCargosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of usuCargoses
    /// </summary>
    public virtual async Task<PagedResultDto<usuCargosDto>> GetListAsync(usuCargosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.usuCargos.usuCargos>, List<usuCargosDto>>(entities);

        return new PagedResultDto<usuCargosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new usuCargos
    /// </summary>
    [Authorize(usuCargosPermissions.Create)]
    public virtual async Task<usuCargosDto> CreateAsync(CreateUpdateusuCargosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateusuCargosDto, Sapienza.Lexus.usuCargos.usuCargos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuCargos.usuCargos, usuCargosDto>(entity);
    }

    /// <summary>
    /// Updates an existing usuCargos
    /// </summary>
    [Authorize(usuCargosPermissions.Update)]
    public virtual async Task<usuCargosDto> UpdateAsync(Guid id, CreateUpdateusuCargosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.usuCargos.usuCargos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuCargos.usuCargos, usuCargosDto>(entity);
    }

    /// <summary>
    /// Deletes a usuCargos
    /// </summary>
    [Authorize(usuCargosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetusuCargosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.usuCargos.usuCargos> ApplyFilters(IQueryable<Sapienza.Lexus.usuCargos.usuCargos> queryable, usuCargosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idCargo != null, x => x.idCargo == input.idCargo)
            .WhereIf(input.idArea != null, x => x.idArea == input.idArea)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
