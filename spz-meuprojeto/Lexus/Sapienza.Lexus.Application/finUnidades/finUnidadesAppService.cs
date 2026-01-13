using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finUnidades.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finUnidades;

/// <summary>
/// Application service for finUnidades entity
/// </summary>
[Authorize(finUnidadesPermissions.Default)]
public class finUnidadesAppService :
    LexusAppService,
    IfinUnidadesAppService
{
    private readonly IRepository<Sapienza.Lexus.finUnidades.finUnidades, Guid> _repository;

    public finUnidadesAppService(
        IRepository<Sapienza.Lexus.finUnidades.finUnidades, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finUnidades by Id
    /// </summary>
    public virtual async Task<finUnidadesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finUnidades.finUnidades, finUnidadesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finUnidadeses
    /// </summary>
    public virtual async Task<PagedResultDto<finUnidadesDto>> GetListAsync(finUnidadesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finUnidades.finUnidades>, List<finUnidadesDto>>(entities);

        return new PagedResultDto<finUnidadesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finUnidades
    /// </summary>
    [Authorize(finUnidadesPermissions.Create)]
    public virtual async Task<finUnidadesDto> CreateAsync(CreateUpdatefinUnidadesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinUnidadesDto, Sapienza.Lexus.finUnidades.finUnidades>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finUnidades.finUnidades, finUnidadesDto>(entity);
    }

    /// <summary>
    /// Updates an existing finUnidades
    /// </summary>
    [Authorize(finUnidadesPermissions.Update)]
    public virtual async Task<finUnidadesDto> UpdateAsync(Guid id, CreateUpdatefinUnidadesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finUnidades.finUnidades), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finUnidades.finUnidades, finUnidadesDto>(entity);
    }

    /// <summary>
    /// Deletes a finUnidades
    /// </summary>
    [Authorize(finUnidadesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinUnidadesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finUnidades.finUnidades> ApplyFilters(IQueryable<Sapienza.Lexus.finUnidades.finUnidades> queryable, finUnidadesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idUnidade != null, x => x.idUnidade == input.idUnidade)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.percentual != null, x => x.percentual == input.percentual)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
