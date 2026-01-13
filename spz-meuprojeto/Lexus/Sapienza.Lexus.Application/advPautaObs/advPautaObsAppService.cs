using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPautaObs.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPautaObs;

/// <summary>
/// Application service for advPautaObs entity
/// </summary>
[Authorize(advPautaObsPermissions.Default)]
public class advPautaObsAppService :
    LexusAppService,
    IadvPautaObsAppService
{
    private readonly IRepository<Sapienza.Lexus.advPautaObs.advPautaObs, Guid> _repository;

    public advPautaObsAppService(
        IRepository<Sapienza.Lexus.advPautaObs.advPautaObs, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPautaObs by Id
    /// </summary>
    public virtual async Task<advPautaObsDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPautaObs.advPautaObs, advPautaObsDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPautaObses
    /// </summary>
    public virtual async Task<PagedResultDto<advPautaObsDto>> GetListAsync(advPautaObsGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPautaObs.advPautaObs>, List<advPautaObsDto>>(entities);

        return new PagedResultDto<advPautaObsDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPautaObs
    /// </summary>
    [Authorize(advPautaObsPermissions.Create)]
    public virtual async Task<advPautaObsDto> CreateAsync(CreateUpdateadvPautaObsDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPautaObsDto, Sapienza.Lexus.advPautaObs.advPautaObs>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPautaObs.advPautaObs, advPautaObsDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPautaObs
    /// </summary>
    [Authorize(advPautaObsPermissions.Update)]
    public virtual async Task<advPautaObsDto> UpdateAsync(Guid id, CreateUpdateadvPautaObsDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPautaObs.advPautaObs), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPautaObs.advPautaObs, advPautaObsDto>(entity);
    }

    /// <summary>
    /// Deletes a advPautaObs
    /// </summary>
    [Authorize(advPautaObsPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPautaObsLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.idTipo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advPautaObs.advPautaObs> ApplyFilters(IQueryable<Sapienza.Lexus.advPautaObs.advPautaObs> queryable, advPautaObsGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.idTipo.Contains(input.Filter) || x.observacao.Contains(input.Filter))
            .WhereIf(input.idPautaObs != null, x => x.idPautaObs == input.idPautaObs)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.id != null, x => x.id == input.id)
            .WhereIf(!input.idTipo.IsNullOrWhiteSpace(), x => x.idTipo.Contains(input.idTipo))
            .WhereIf(!input.observacao.IsNullOrWhiteSpace(), x => x.observacao.Contains(input.observacao))
            // ========== FK Filters ==========
            ;
    }
}
