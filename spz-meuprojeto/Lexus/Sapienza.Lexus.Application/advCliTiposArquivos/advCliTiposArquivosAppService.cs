using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCliTiposArquivos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliTiposArquivos;

/// <summary>
/// Application service for advCliTiposArquivos entity
/// </summary>
[Authorize(advCliTiposArquivosPermissions.Default)]
public class advCliTiposArquivosAppService :
    LexusAppService,
    IadvCliTiposArquivosAppService
{
    private readonly IRepository<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos, Guid> _repository;

    public advCliTiposArquivosAppService(
        IRepository<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advCliTiposArquivos by Id
    /// </summary>
    public virtual async Task<advCliTiposArquivosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos, advCliTiposArquivosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCliTiposArquivoses
    /// </summary>
    public virtual async Task<PagedResultDto<advCliTiposArquivosDto>> GetListAsync(advCliTiposArquivosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos>, List<advCliTiposArquivosDto>>(entities);

        return new PagedResultDto<advCliTiposArquivosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCliTiposArquivos
    /// </summary>
    [Authorize(advCliTiposArquivosPermissions.Create)]
    public virtual async Task<advCliTiposArquivosDto> CreateAsync(CreateUpdateadvCliTiposArquivosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCliTiposArquivosDto, Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos, advCliTiposArquivosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCliTiposArquivos
    /// </summary>
    [Authorize(advCliTiposArquivosPermissions.Update)]
    public virtual async Task<advCliTiposArquivosDto> UpdateAsync(Guid id, CreateUpdateadvCliTiposArquivosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos, advCliTiposArquivosDto>(entity);
    }

    /// <summary>
    /// Deletes a advCliTiposArquivos
    /// </summary>
    [Authorize(advCliTiposArquivosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCliTiposArquivosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos> ApplyFilters(IQueryable<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos> queryable, advCliTiposArquivosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.pasta.Contains(input.Filter))
            .WhereIf(input.idTipoArquivo != null, x => x.idTipoArquivo == input.idTipoArquivo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.pasta.IsNullOrWhiteSpace(), x => x.pasta.Contains(input.pasta))
            // ========== FK Filters ==========
            ;
    }
}
