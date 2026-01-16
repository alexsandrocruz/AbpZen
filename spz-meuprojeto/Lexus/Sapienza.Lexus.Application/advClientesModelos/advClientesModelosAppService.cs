using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advClientesModelos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advClientesModelos;

/// <summary>
/// Application service for advClientesModelos entity
/// </summary>
[Authorize(advClientesModelosPermissions.Default)]
public class advClientesModelosAppService :
    LexusAppService,
    IadvClientesModelosAppService
{
    private readonly IRepository<Sapienza.Lexus.advClientesModelos.advClientesModelos, Guid> _repository;

    public advClientesModelosAppService(
        IRepository<Sapienza.Lexus.advClientesModelos.advClientesModelos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advClientesModelos by Id
    /// </summary>
    public virtual async Task<advClientesModelosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advClientesModelos.advClientesModelos, advClientesModelosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advClientesModeloses
    /// </summary>
    public virtual async Task<PagedResultDto<advClientesModelosDto>> GetListAsync(advClientesModelosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advClientesModelos.advClientesModelos>, List<advClientesModelosDto>>(entities);

        return new PagedResultDto<advClientesModelosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advClientesModelos
    /// </summary>
    [Authorize(advClientesModelosPermissions.Create)]
    public virtual async Task<advClientesModelosDto> CreateAsync(CreateUpdateadvClientesModelosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvClientesModelosDto, Sapienza.Lexus.advClientesModelos.advClientesModelos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesModelos.advClientesModelos, advClientesModelosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advClientesModelos
    /// </summary>
    [Authorize(advClientesModelosPermissions.Update)]
    public virtual async Task<advClientesModelosDto> UpdateAsync(Guid id, CreateUpdateadvClientesModelosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advClientesModelos.advClientesModelos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesModelos.advClientesModelos, advClientesModelosDto>(entity);
    }

    /// <summary>
    /// Deletes a advClientesModelos
    /// </summary>
    [Authorize(advClientesModelosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvClientesModelosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advClientesModelos.advClientesModelos> ApplyFilters(IQueryable<Sapienza.Lexus.advClientesModelos.advClientesModelos> queryable, advClientesModelosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.conteudo.Contains(input.Filter))
            .WhereIf(input.idModelo != null, x => x.idModelo == input.idModelo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(!input.conteudo.IsNullOrWhiteSpace(), x => x.conteudo.Contains(input.conteudo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
