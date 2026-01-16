using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProProbabilidades.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProProbabilidades;

/// <summary>
/// Application service for advProProbabilidades entity
/// </summary>
[Authorize(advProProbabilidadesPermissions.Default)]
public class advProProbabilidadesAppService :
    LexusAppService,
    IadvProProbabilidadesAppService
{
    private readonly IRepository<Sapienza.Lexus.advProProbabilidades.advProProbabilidades, Guid> _repository;

    public advProProbabilidadesAppService(
        IRepository<Sapienza.Lexus.advProProbabilidades.advProProbabilidades, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProProbabilidades by Id
    /// </summary>
    public virtual async Task<advProProbabilidadesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProProbabilidades.advProProbabilidades, advProProbabilidadesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProProbabilidadeses
    /// </summary>
    public virtual async Task<PagedResultDto<advProProbabilidadesDto>> GetListAsync(advProProbabilidadesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProProbabilidades.advProProbabilidades>, List<advProProbabilidadesDto>>(entities);

        return new PagedResultDto<advProProbabilidadesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProProbabilidades
    /// </summary>
    [Authorize(advProProbabilidadesPermissions.Create)]
    public virtual async Task<advProProbabilidadesDto> CreateAsync(CreateUpdateadvProProbabilidadesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProProbabilidadesDto, Sapienza.Lexus.advProProbabilidades.advProProbabilidades>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProProbabilidades.advProProbabilidades, advProProbabilidadesDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProProbabilidades
    /// </summary>
    [Authorize(advProProbabilidadesPermissions.Update)]
    public virtual async Task<advProProbabilidadesDto> UpdateAsync(Guid id, CreateUpdateadvProProbabilidadesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProProbabilidades.advProProbabilidades), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProProbabilidades.advProProbabilidades, advProProbabilidadesDto>(entity);
    }

    /// <summary>
    /// Deletes a advProProbabilidades
    /// </summary>
    [Authorize(advProProbabilidadesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProProbabilidadesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProProbabilidades.advProProbabilidades> ApplyFilters(IQueryable<Sapienza.Lexus.advProProbabilidades.advProProbabilidades> queryable, advProProbabilidadesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idProbabilidade != null, x => x.idProbabilidade == input.idProbabilidade)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
