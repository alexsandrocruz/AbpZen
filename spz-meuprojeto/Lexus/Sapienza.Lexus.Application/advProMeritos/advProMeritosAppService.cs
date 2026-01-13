using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProMeritos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProMeritos;

/// <summary>
/// Application service for advProMeritos entity
/// </summary>
[Authorize(advProMeritosPermissions.Default)]
public class advProMeritosAppService :
    LexusAppService,
    IadvProMeritosAppService
{
    private readonly IRepository<Sapienza.Lexus.advProMeritos.advProMeritos, Guid> _repository;

    public advProMeritosAppService(
        IRepository<Sapienza.Lexus.advProMeritos.advProMeritos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProMeritos by Id
    /// </summary>
    public virtual async Task<advProMeritosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProMeritos.advProMeritos, advProMeritosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProMeritoses
    /// </summary>
    public virtual async Task<PagedResultDto<advProMeritosDto>> GetListAsync(advProMeritosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProMeritos.advProMeritos>, List<advProMeritosDto>>(entities);

        return new PagedResultDto<advProMeritosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProMeritos
    /// </summary>
    [Authorize(advProMeritosPermissions.Create)]
    public virtual async Task<advProMeritosDto> CreateAsync(CreateUpdateadvProMeritosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProMeritosDto, Sapienza.Lexus.advProMeritos.advProMeritos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProMeritos.advProMeritos, advProMeritosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProMeritos
    /// </summary>
    [Authorize(advProMeritosPermissions.Update)]
    public virtual async Task<advProMeritosDto> UpdateAsync(Guid id, CreateUpdateadvProMeritosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProMeritos.advProMeritos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProMeritos.advProMeritos, advProMeritosDto>(entity);
    }

    /// <summary>
    /// Deletes a advProMeritos
    /// </summary>
    [Authorize(advProMeritosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProMeritosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProMeritos.advProMeritos> ApplyFilters(IQueryable<Sapienza.Lexus.advProMeritos.advProMeritos> queryable, advProMeritosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idMerito != null, x => x.idMerito == input.idMerito)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.beneficioINSS != null, x => x.beneficioINSS == input.beneficioINSS)
            // ========== FK Filters ==========
            ;
    }
}
