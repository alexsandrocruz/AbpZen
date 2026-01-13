using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.flwConfigExcecoes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.flwConfigExcecoes;

/// <summary>
/// Application service for flwConfigExcecoes entity
/// </summary>
[Authorize(flwConfigExcecoesPermissions.Default)]
public class flwConfigExcecoesAppService :
    LexusAppService,
    IflwConfigExcecoesAppService
{
    private readonly IRepository<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes, Guid> _repository;

    public flwConfigExcecoesAppService(
        IRepository<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single flwConfigExcecoes by Id
    /// </summary>
    public virtual async Task<flwConfigExcecoesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes, flwConfigExcecoesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of flwConfigExcecoeses
    /// </summary>
    public virtual async Task<PagedResultDto<flwConfigExcecoesDto>> GetListAsync(flwConfigExcecoesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes>, List<flwConfigExcecoesDto>>(entities);

        return new PagedResultDto<flwConfigExcecoesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new flwConfigExcecoes
    /// </summary>
    [Authorize(flwConfigExcecoesPermissions.Create)]
    public virtual async Task<flwConfigExcecoesDto> CreateAsync(CreateUpdateflwConfigExcecoesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateflwConfigExcecoesDto, Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes, flwConfigExcecoesDto>(entity);
    }

    /// <summary>
    /// Updates an existing flwConfigExcecoes
    /// </summary>
    [Authorize(flwConfigExcecoesPermissions.Update)]
    public virtual async Task<flwConfigExcecoesDto> UpdateAsync(Guid id, CreateUpdateflwConfigExcecoesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes, flwConfigExcecoesDto>(entity);
    }

    /// <summary>
    /// Deletes a flwConfigExcecoes
    /// </summary>
    [Authorize(flwConfigExcecoesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetflwConfigExcecoesLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.tipoMarcacoes
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes> ApplyFilters(IQueryable<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes> queryable, flwConfigExcecoesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.tipoMarcacoes.Contains(input.Filter) || x.data.Contains(input.Filter))
            .WhereIf(input.idConfig != null, x => x.idConfig == input.idConfig)
            .WhereIf(!input.tipoMarcacoes.IsNullOrWhiteSpace(), x => x.tipoMarcacoes.Contains(input.tipoMarcacoes))
            .WhereIf(input.idHistoricoTipo != null, x => x.idHistoricoTipo == input.idHistoricoTipo)
            .WhereIf(!input.data.IsNullOrWhiteSpace(), x => x.data.Contains(input.data))
            .WhereIf(input.qtde != null, x => x.qtde == input.qtde)
            .WhereIf(input.manhaQtde != null, x => x.manhaQtde == input.manhaQtde)
            .WhereIf(input.tardeQtde != null, x => x.tardeQtde == input.tardeQtde)
            // ========== FK Filters ==========
            ;
    }
}
