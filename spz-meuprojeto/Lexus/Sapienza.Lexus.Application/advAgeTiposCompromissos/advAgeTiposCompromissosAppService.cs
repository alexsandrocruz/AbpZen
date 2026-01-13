using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advAgeTiposCompromissos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advAgeTiposCompromissos;

/// <summary>
/// Application service for advAgeTiposCompromissos entity
/// </summary>
[Authorize(advAgeTiposCompromissosPermissions.Default)]
public class advAgeTiposCompromissosAppService :
    LexusAppService,
    IadvAgeTiposCompromissosAppService
{
    private readonly IRepository<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos, Guid> _repository;

    public advAgeTiposCompromissosAppService(
        IRepository<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advAgeTiposCompromissos by Id
    /// </summary>
    public virtual async Task<advAgeTiposCompromissosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos, advAgeTiposCompromissosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advAgeTiposCompromissoses
    /// </summary>
    public virtual async Task<PagedResultDto<advAgeTiposCompromissosDto>> GetListAsync(advAgeTiposCompromissosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos>, List<advAgeTiposCompromissosDto>>(entities);

        return new PagedResultDto<advAgeTiposCompromissosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advAgeTiposCompromissos
    /// </summary>
    [Authorize(advAgeTiposCompromissosPermissions.Create)]
    public virtual async Task<advAgeTiposCompromissosDto> CreateAsync(CreateUpdateadvAgeTiposCompromissosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvAgeTiposCompromissosDto, Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos, advAgeTiposCompromissosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advAgeTiposCompromissos
    /// </summary>
    [Authorize(advAgeTiposCompromissosPermissions.Update)]
    public virtual async Task<advAgeTiposCompromissosDto> UpdateAsync(Guid id, CreateUpdateadvAgeTiposCompromissosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos, advAgeTiposCompromissosDto>(entity);
    }

    /// <summary>
    /// Deletes a advAgeTiposCompromissos
    /// </summary>
    [Authorize(advAgeTiposCompromissosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvAgeTiposCompromissosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos> ApplyFilters(IQueryable<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos> queryable, advAgeTiposCompromissosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idTipoCompromisso != null, x => x.idTipoCompromisso == input.idTipoCompromisso)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.recebimentoProcesso != null, x => x.recebimentoProcesso == input.recebimentoProcesso)
            // ========== FK Filters ==========
            ;
    }
}
