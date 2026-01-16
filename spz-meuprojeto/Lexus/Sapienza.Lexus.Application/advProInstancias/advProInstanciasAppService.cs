using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProInstancias.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProInstancias;

/// <summary>
/// Application service for advProInstancias entity
/// </summary>
[Authorize(advProInstanciasPermissions.Default)]
public class advProInstanciasAppService :
    LexusAppService,
    IadvProInstanciasAppService
{
    private readonly IRepository<Sapienza.Lexus.advProInstancias.advProInstancias, Guid> _repository;

    public advProInstanciasAppService(
        IRepository<Sapienza.Lexus.advProInstancias.advProInstancias, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProInstancias by Id
    /// </summary>
    public virtual async Task<advProInstanciasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProInstancias.advProInstancias, advProInstanciasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProInstanciases
    /// </summary>
    public virtual async Task<PagedResultDto<advProInstanciasDto>> GetListAsync(advProInstanciasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProInstancias.advProInstancias>, List<advProInstanciasDto>>(entities);

        return new PagedResultDto<advProInstanciasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProInstancias
    /// </summary>
    [Authorize(advProInstanciasPermissions.Create)]
    public virtual async Task<advProInstanciasDto> CreateAsync(CreateUpdateadvProInstanciasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProInstanciasDto, Sapienza.Lexus.advProInstancias.advProInstancias>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProInstancias.advProInstancias, advProInstanciasDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProInstancias
    /// </summary>
    [Authorize(advProInstanciasPermissions.Update)]
    public virtual async Task<advProInstanciasDto> UpdateAsync(Guid id, CreateUpdateadvProInstanciasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProInstancias.advProInstancias), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProInstancias.advProInstancias, advProInstanciasDto>(entity);
    }

    /// <summary>
    /// Deletes a advProInstancias
    /// </summary>
    [Authorize(advProInstanciasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProInstanciasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProInstancias.advProInstancias> ApplyFilters(IQueryable<Sapienza.Lexus.advProInstancias.advProInstancias> queryable, advProInstanciasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idInstancia != null, x => x.idInstancia == input.idInstancia)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
