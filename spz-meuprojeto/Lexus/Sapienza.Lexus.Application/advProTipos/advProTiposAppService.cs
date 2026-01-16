using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProTipos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProTipos;

/// <summary>
/// Application service for advProTipos entity
/// </summary>
[Authorize(advProTiposPermissions.Default)]
public class advProTiposAppService :
    LexusAppService,
    IadvProTiposAppService
{
    private readonly IRepository<Sapienza.Lexus.advProTipos.advProTipos, Guid> _repository;

    public advProTiposAppService(
        IRepository<Sapienza.Lexus.advProTipos.advProTipos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProTipos by Id
    /// </summary>
    public virtual async Task<advProTiposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProTipos.advProTipos, advProTiposDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProTiposes
    /// </summary>
    public virtual async Task<PagedResultDto<advProTiposDto>> GetListAsync(advProTiposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProTipos.advProTipos>, List<advProTiposDto>>(entities);

        return new PagedResultDto<advProTiposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProTipos
    /// </summary>
    [Authorize(advProTiposPermissions.Create)]
    public virtual async Task<advProTiposDto> CreateAsync(CreateUpdateadvProTiposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProTiposDto, Sapienza.Lexus.advProTipos.advProTipos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProTipos.advProTipos, advProTiposDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProTipos
    /// </summary>
    [Authorize(advProTiposPermissions.Update)]
    public virtual async Task<advProTiposDto> UpdateAsync(Guid id, CreateUpdateadvProTiposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProTipos.advProTipos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProTipos.advProTipos, advProTiposDto>(entity);
    }

    /// <summary>
    /// Deletes a advProTipos
    /// </summary>
    [Authorize(advProTiposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProTiposLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProTipos.advProTipos> ApplyFilters(IQueryable<Sapienza.Lexus.advProTipos.advProTipos> queryable, advProTiposGetListInput input)
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
