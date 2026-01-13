using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProEscritorios.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProEscritorios;

/// <summary>
/// Application service for advProEscritorios entity
/// </summary>
[Authorize(advProEscritoriosPermissions.Default)]
public class advProEscritoriosAppService :
    LexusAppService,
    IadvProEscritoriosAppService
{
    private readonly IRepository<Sapienza.Lexus.advProEscritorios.advProEscritorios, Guid> _repository;

    public advProEscritoriosAppService(
        IRepository<Sapienza.Lexus.advProEscritorios.advProEscritorios, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProEscritorios by Id
    /// </summary>
    public virtual async Task<advProEscritoriosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProEscritorios.advProEscritorios, advProEscritoriosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProEscritorioses
    /// </summary>
    public virtual async Task<PagedResultDto<advProEscritoriosDto>> GetListAsync(advProEscritoriosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProEscritorios.advProEscritorios>, List<advProEscritoriosDto>>(entities);

        return new PagedResultDto<advProEscritoriosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProEscritorios
    /// </summary>
    [Authorize(advProEscritoriosPermissions.Create)]
    public virtual async Task<advProEscritoriosDto> CreateAsync(CreateUpdateadvProEscritoriosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProEscritoriosDto, Sapienza.Lexus.advProEscritorios.advProEscritorios>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProEscritorios.advProEscritorios, advProEscritoriosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProEscritorios
    /// </summary>
    [Authorize(advProEscritoriosPermissions.Update)]
    public virtual async Task<advProEscritoriosDto> UpdateAsync(Guid id, CreateUpdateadvProEscritoriosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProEscritorios.advProEscritorios), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProEscritorios.advProEscritorios, advProEscritoriosDto>(entity);
    }

    /// <summary>
    /// Deletes a advProEscritorios
    /// </summary>
    [Authorize(advProEscritoriosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProEscritoriosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProEscritorios.advProEscritorios> ApplyFilters(IQueryable<Sapienza.Lexus.advProEscritorios.advProEscritorios> queryable, advProEscritoriosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idEscritorio != null, x => x.idEscritorio == input.idEscritorio)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idCentroCusto != null, x => x.idCentroCusto == input.idCentroCusto)
            // ========== FK Filters ==========
            ;
    }
}
