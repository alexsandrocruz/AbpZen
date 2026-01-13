using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPreStatusTipos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreStatusTipos;

/// <summary>
/// Application service for advPreStatusTipos entity
/// </summary>
[Authorize(advPreStatusTiposPermissions.Default)]
public class advPreStatusTiposAppService :
    LexusAppService,
    IadvPreStatusTiposAppService
{
    private readonly IRepository<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos, Guid> _repository;

    public advPreStatusTiposAppService(
        IRepository<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPreStatusTipos by Id
    /// </summary>
    public virtual async Task<advPreStatusTiposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos, advPreStatusTiposDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPreStatusTiposes
    /// </summary>
    public virtual async Task<PagedResultDto<advPreStatusTiposDto>> GetListAsync(advPreStatusTiposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos>, List<advPreStatusTiposDto>>(entities);

        return new PagedResultDto<advPreStatusTiposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPreStatusTipos
    /// </summary>
    [Authorize(advPreStatusTiposPermissions.Create)]
    public virtual async Task<advPreStatusTiposDto> CreateAsync(CreateUpdateadvPreStatusTiposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPreStatusTiposDto, Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos, advPreStatusTiposDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPreStatusTipos
    /// </summary>
    [Authorize(advPreStatusTiposPermissions.Update)]
    public virtual async Task<advPreStatusTiposDto> UpdateAsync(Guid id, CreateUpdateadvPreStatusTiposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos, advPreStatusTiposDto>(entity);
    }

    /// <summary>
    /// Deletes a advPreStatusTipos
    /// </summary>
    [Authorize(advPreStatusTiposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPreStatusTiposLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos> ApplyFilters(IQueryable<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos> queryable, advPreStatusTiposGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idTipo != null, x => x.idTipo == input.idTipo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
