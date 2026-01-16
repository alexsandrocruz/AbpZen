using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCliLocaisAtendido.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliLocaisAtendido;

/// <summary>
/// Application service for advCliLocaisAtendido entity
/// </summary>
[Authorize(advCliLocaisAtendidoPermissions.Default)]
public class advCliLocaisAtendidoAppService :
    LexusAppService,
    IadvCliLocaisAtendidoAppService
{
    private readonly IRepository<Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido, Guid> _repository;

    public advCliLocaisAtendidoAppService(
        IRepository<Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advCliLocaisAtendido by Id
    /// </summary>
    public virtual async Task<advCliLocaisAtendidoDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido, advCliLocaisAtendidoDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCliLocaisAtendidos
    /// </summary>
    public virtual async Task<PagedResultDto<advCliLocaisAtendidoDto>> GetListAsync(advCliLocaisAtendidoGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido>, List<advCliLocaisAtendidoDto>>(entities);

        return new PagedResultDto<advCliLocaisAtendidoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCliLocaisAtendido
    /// </summary>
    [Authorize(advCliLocaisAtendidoPermissions.Create)]
    public virtual async Task<advCliLocaisAtendidoDto> CreateAsync(CreateUpdateadvCliLocaisAtendidoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCliLocaisAtendidoDto, Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido, advCliLocaisAtendidoDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCliLocaisAtendido
    /// </summary>
    [Authorize(advCliLocaisAtendidoPermissions.Update)]
    public virtual async Task<advCliLocaisAtendidoDto> UpdateAsync(Guid id, CreateUpdateadvCliLocaisAtendidoDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido, advCliLocaisAtendidoDto>(entity);
    }

    /// <summary>
    /// Deletes a advCliLocaisAtendido
    /// </summary>
    [Authorize(advCliLocaisAtendidoPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCliLocaisAtendidoLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido> ApplyFilters(IQueryable<Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido> queryable, advCliLocaisAtendidoGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idLocalAtendido != null, x => x.idLocalAtendido == input.idLocalAtendido)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
