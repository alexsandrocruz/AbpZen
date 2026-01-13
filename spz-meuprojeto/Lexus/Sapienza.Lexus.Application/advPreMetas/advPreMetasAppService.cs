using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPreMetas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreMetas;

/// <summary>
/// Application service for advPreMetas entity
/// </summary>
[Authorize(advPreMetasPermissions.Default)]
public class advPreMetasAppService :
    LexusAppService,
    IadvPreMetasAppService
{
    private readonly IRepository<Sapienza.Lexus.advPreMetas.advPreMetas, Guid> _repository;

    public advPreMetasAppService(
        IRepository<Sapienza.Lexus.advPreMetas.advPreMetas, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPreMetas by Id
    /// </summary>
    public virtual async Task<advPreMetasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPreMetas.advPreMetas, advPreMetasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPreMetases
    /// </summary>
    public virtual async Task<PagedResultDto<advPreMetasDto>> GetListAsync(advPreMetasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPreMetas.advPreMetas>, List<advPreMetasDto>>(entities);

        return new PagedResultDto<advPreMetasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPreMetas
    /// </summary>
    [Authorize(advPreMetasPermissions.Create)]
    public virtual async Task<advPreMetasDto> CreateAsync(CreateUpdateadvPreMetasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPreMetasDto, Sapienza.Lexus.advPreMetas.advPreMetas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreMetas.advPreMetas, advPreMetasDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPreMetas
    /// </summary>
    [Authorize(advPreMetasPermissions.Update)]
    public virtual async Task<advPreMetasDto> UpdateAsync(Guid id, CreateUpdateadvPreMetasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPreMetas.advPreMetas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreMetas.advPreMetas, advPreMetasDto>(entity);
    }

    /// <summary>
    /// Deletes a advPreMetas
    /// </summary>
    [Authorize(advPreMetasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPreMetasLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.tipo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advPreMetas.advPreMetas> ApplyFilters(IQueryable<Sapienza.Lexus.advPreMetas.advPreMetas> queryable, advPreMetasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.tipo.Contains(input.Filter))
            .WhereIf(input.idMeta != null, x => x.idMeta == input.idMeta)
            .WhereIf(!input.tipo.IsNullOrWhiteSpace(), x => x.tipo.Contains(input.tipo))
            .WhereIf(input.idResponsavel != null, x => x.idResponsavel == input.idResponsavel)
            .WhereIf(input.idEscritorio != null, x => x.idEscritorio == input.idEscritorio)
            .WhereIf(input.qtde != null, x => x.qtde == input.qtde)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            // ========== FK Filters ==========
            ;
    }
}
