using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advClientesHistoricos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advClientesHistoricos;

/// <summary>
/// Application service for advClientesHistoricos entity
/// </summary>
[Authorize(advClientesHistoricosPermissions.Default)]
public class advClientesHistoricosAppService :
    LexusAppService,
    IadvClientesHistoricosAppService
{
    private readonly IRepository<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos, Guid> _repository;

    public advClientesHistoricosAppService(
        IRepository<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advClientesHistoricos by Id
    /// </summary>
    public virtual async Task<advClientesHistoricosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos, advClientesHistoricosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advClientesHistoricoses
    /// </summary>
    public virtual async Task<PagedResultDto<advClientesHistoricosDto>> GetListAsync(advClientesHistoricosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos>, List<advClientesHistoricosDto>>(entities);

        return new PagedResultDto<advClientesHistoricosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advClientesHistoricos
    /// </summary>
    [Authorize(advClientesHistoricosPermissions.Create)]
    public virtual async Task<advClientesHistoricosDto> CreateAsync(CreateUpdateadvClientesHistoricosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvClientesHistoricosDto, Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos, advClientesHistoricosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advClientesHistoricos
    /// </summary>
    [Authorize(advClientesHistoricosPermissions.Update)]
    public virtual async Task<advClientesHistoricosDto> UpdateAsync(Guid id, CreateUpdateadvClientesHistoricosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos, advClientesHistoricosDto>(entity);
    }

    /// <summary>
    /// Deletes a advClientesHistoricos
    /// </summary>
    [Authorize(advClientesHistoricosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvClientesHistoricosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.data
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos> ApplyFilters(IQueryable<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos> queryable, advClientesHistoricosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.data.Contains(input.Filter) || x.hora.Contains(input.Filter) || x.ocorrencia.Contains(input.Filter) || x.depto.Contains(input.Filter))
            .WhereIf(input.idHistorico != null, x => x.idHistorico == input.idHistorico)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.IdentityUserId != null, x => x.IdentityUserId == input.IdentityUserId)
            .WhereIf(input.idTipoHistorico != null, x => x.idTipoHistorico == input.idTipoHistorico)
            .WhereIf(!input.data.IsNullOrWhiteSpace(), x => x.data.Contains(input.data))
            .WhereIf(!input.hora.IsNullOrWhiteSpace(), x => x.hora.Contains(input.hora))
            .WhereIf(!input.ocorrencia.IsNullOrWhiteSpace(), x => x.ocorrencia.Contains(input.ocorrencia))
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.idOportunidade != null, x => x.idOportunidade == input.idOportunidade)
            .WhereIf(!input.depto.IsNullOrWhiteSpace(), x => x.depto.Contains(input.depto))
            .WhereIf(input.prioritario != null, x => x.prioritario == input.prioritario)
            // ========== FK Filters ==========
            ;
    }
}
