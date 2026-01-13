using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCliBairros.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliBairros;

/// <summary>
/// Application service for advCliBairros entity
/// </summary>
[Authorize(advCliBairrosPermissions.Default)]
public class advCliBairrosAppService :
    LexusAppService,
    IadvCliBairrosAppService
{
    private readonly IRepository<Sapienza.Lexus.advCliBairros.advCliBairros, Guid> _repository;

    public advCliBairrosAppService(
        IRepository<Sapienza.Lexus.advCliBairros.advCliBairros, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advCliBairros by Id
    /// </summary>
    public virtual async Task<advCliBairrosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCliBairros.advCliBairros, advCliBairrosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCliBairroses
    /// </summary>
    public virtual async Task<PagedResultDto<advCliBairrosDto>> GetListAsync(advCliBairrosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCliBairros.advCliBairros>, List<advCliBairrosDto>>(entities);

        return new PagedResultDto<advCliBairrosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCliBairros
    /// </summary>
    [Authorize(advCliBairrosPermissions.Create)]
    public virtual async Task<advCliBairrosDto> CreateAsync(CreateUpdateadvCliBairrosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCliBairrosDto, Sapienza.Lexus.advCliBairros.advCliBairros>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliBairros.advCliBairros, advCliBairrosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCliBairros
    /// </summary>
    [Authorize(advCliBairrosPermissions.Update)]
    public virtual async Task<advCliBairrosDto> UpdateAsync(Guid id, CreateUpdateadvCliBairrosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCliBairros.advCliBairros), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliBairros.advCliBairros, advCliBairrosDto>(entity);
    }

    /// <summary>
    /// Deletes a advCliBairros
    /// </summary>
    [Authorize(advCliBairrosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCliBairrosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advCliBairros.advCliBairros> ApplyFilters(IQueryable<Sapienza.Lexus.advCliBairros.advCliBairros> queryable, advCliBairrosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.cidade.Contains(input.Filter) || x.estado.Contains(input.Filter))
            .WhereIf(input.idBairro != null, x => x.idBairro == input.idBairro)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(!input.cidade.IsNullOrWhiteSpace(), x => x.cidade.Contains(input.cidade))
            .WhereIf(!input.estado.IsNullOrWhiteSpace(), x => x.estado.Contains(input.estado))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
