using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProfissionaisNaturezas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProfissionaisNaturezas;

/// <summary>
/// Application service for advProfissionaisNaturezas entity
/// </summary>
[Authorize(advProfissionaisNaturezasPermissions.Default)]
public class advProfissionaisNaturezasAppService :
    LexusAppService,
    IadvProfissionaisNaturezasAppService
{
    private readonly IRepository<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas, Guid> _repository;

    public advProfissionaisNaturezasAppService(
        IRepository<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProfissionaisNaturezas by Id
    /// </summary>
    public virtual async Task<advProfissionaisNaturezasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas, advProfissionaisNaturezasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProfissionaisNaturezases
    /// </summary>
    public virtual async Task<PagedResultDto<advProfissionaisNaturezasDto>> GetListAsync(advProfissionaisNaturezasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas>, List<advProfissionaisNaturezasDto>>(entities);

        return new PagedResultDto<advProfissionaisNaturezasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProfissionaisNaturezas
    /// </summary>
    [Authorize(advProfissionaisNaturezasPermissions.Create)]
    public virtual async Task<advProfissionaisNaturezasDto> CreateAsync(CreateUpdateadvProfissionaisNaturezasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProfissionaisNaturezasDto, Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas, advProfissionaisNaturezasDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProfissionaisNaturezas
    /// </summary>
    [Authorize(advProfissionaisNaturezasPermissions.Update)]
    public virtual async Task<advProfissionaisNaturezasDto> UpdateAsync(Guid id, CreateUpdateadvProfissionaisNaturezasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas, advProfissionaisNaturezasDto>(entity);
    }

    /// <summary>
    /// Deletes a advProfissionaisNaturezas
    /// </summary>
    [Authorize(advProfissionaisNaturezasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProfissionaisNaturezasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas> ApplyFilters(IQueryable<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas> queryable, advProfissionaisNaturezasGetListInput input)
    {
        return queryable
            .WhereIf(input.idProfissionalNatureza != null, x => x.idProfissionalNatureza == input.idProfissionalNatureza)
            .WhereIf(input.idProfissional != null, x => x.idProfissional == input.idProfissional)
            .WhereIf(input.idNatureza != null, x => x.idNatureza == input.idNatureza)
            // ========== FK Filters ==========
            ;
    }
}
