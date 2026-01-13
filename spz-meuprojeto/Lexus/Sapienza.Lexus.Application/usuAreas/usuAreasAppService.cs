using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.usuAreas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.usuAreas;

/// <summary>
/// Application service for usuAreas entity
/// </summary>
[Authorize(usuAreasPermissions.Default)]
public class usuAreasAppService :
    LexusAppService,
    IusuAreasAppService
{
    private readonly IRepository<Sapienza.Lexus.usuAreas.usuAreas, Guid> _repository;

    public usuAreasAppService(
        IRepository<Sapienza.Lexus.usuAreas.usuAreas, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single usuAreas by Id
    /// </summary>
    public virtual async Task<usuAreasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.usuAreas.usuAreas, usuAreasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of usuAreases
    /// </summary>
    public virtual async Task<PagedResultDto<usuAreasDto>> GetListAsync(usuAreasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.usuAreas.usuAreas>, List<usuAreasDto>>(entities);

        return new PagedResultDto<usuAreasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new usuAreas
    /// </summary>
    [Authorize(usuAreasPermissions.Create)]
    public virtual async Task<usuAreasDto> CreateAsync(CreateUpdateusuAreasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateusuAreasDto, Sapienza.Lexus.usuAreas.usuAreas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuAreas.usuAreas, usuAreasDto>(entity);
    }

    /// <summary>
    /// Updates an existing usuAreas
    /// </summary>
    [Authorize(usuAreasPermissions.Update)]
    public virtual async Task<usuAreasDto> UpdateAsync(Guid id, CreateUpdateusuAreasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.usuAreas.usuAreas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuAreas.usuAreas, usuAreasDto>(entity);
    }

    /// <summary>
    /// Deletes a usuAreas
    /// </summary>
    [Authorize(usuAreasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetusuAreasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.usuAreas.usuAreas> ApplyFilters(IQueryable<Sapienza.Lexus.usuAreas.usuAreas> queryable, usuAreasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idArea != null, x => x.idArea == input.idArea)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
