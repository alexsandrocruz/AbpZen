using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabLembretes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabLembretes;

/// <summary>
/// Application service for fabLembretes entity
/// </summary>
[Authorize(fabLembretesPermissions.Default)]
public class fabLembretesAppService :
    LexusAppService,
    IfabLembretesAppService
{
    private readonly IRepository<Sapienza.Lexus.fabLembretes.fabLembretes, Guid> _repository;

    public fabLembretesAppService(
        IRepository<Sapienza.Lexus.fabLembretes.fabLembretes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fabLembretes by Id
    /// </summary>
    public virtual async Task<fabLembretesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabLembretes.fabLembretes, fabLembretesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabLembreteses
    /// </summary>
    public virtual async Task<PagedResultDto<fabLembretesDto>> GetListAsync(fabLembretesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabLembretes.fabLembretes>, List<fabLembretesDto>>(entities);

        return new PagedResultDto<fabLembretesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabLembretes
    /// </summary>
    [Authorize(fabLembretesPermissions.Create)]
    public virtual async Task<fabLembretesDto> CreateAsync(CreateUpdatefabLembretesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabLembretesDto, Sapienza.Lexus.fabLembretes.fabLembretes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabLembretes.fabLembretes, fabLembretesDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabLembretes
    /// </summary>
    [Authorize(fabLembretesPermissions.Update)]
    public virtual async Task<fabLembretesDto> UpdateAsync(Guid id, CreateUpdatefabLembretesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabLembretes.fabLembretes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabLembretes.fabLembretes, fabLembretesDto>(entity);
    }

    /// <summary>
    /// Deletes a fabLembretes
    /// </summary>
    [Authorize(fabLembretesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabLembretesLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.mensagem
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.fabLembretes.fabLembretes> ApplyFilters(IQueryable<Sapienza.Lexus.fabLembretes.fabLembretes> queryable, fabLembretesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.mensagem.Contains(input.Filter) || x.destino.Contains(input.Filter) || x.incluidoPor.Contains(input.Filter) || x.tipo.Contains(input.Filter))
            .WhereIf(input.idLembrete != null, x => x.idLembrete == input.idLembrete)
            .WhereIf(input.idUsuario != null, x => x.idUsuario == input.idUsuario)
            .WhereIf(!input.mensagem.IsNullOrWhiteSpace(), x => x.mensagem.Contains(input.mensagem))
            .WhereIf(!input.destino.IsNullOrWhiteSpace(), x => x.destino.Contains(input.destino))
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(!input.incluidoPor.IsNullOrWhiteSpace(), x => x.incluidoPor.Contains(input.incluidoPor))
            .WhereIf(input.lido != null, x => x.lido == input.lido)
            .WhereIf(!input.tipo.IsNullOrWhiteSpace(), x => x.tipo.Contains(input.tipo))
            // ========== FK Filters ==========
            ;
    }
}
