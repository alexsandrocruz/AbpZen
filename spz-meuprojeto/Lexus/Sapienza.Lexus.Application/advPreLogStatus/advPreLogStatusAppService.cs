using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPreLogStatus.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreLogStatus;

/// <summary>
/// Application service for advPreLogStatus entity
/// </summary>
[Authorize(advPreLogStatusPermissions.Default)]
public class advPreLogStatusAppService :
    LexusAppService,
    IadvPreLogStatusAppService
{
    private readonly IRepository<Sapienza.Lexus.advPreLogStatus.advPreLogStatus, Guid> _repository;

    public advPreLogStatusAppService(
        IRepository<Sapienza.Lexus.advPreLogStatus.advPreLogStatus, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPreLogStatus by Id
    /// </summary>
    public virtual async Task<advPreLogStatusDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPreLogStatus.advPreLogStatus, advPreLogStatusDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPreLogStatuses
    /// </summary>
    public virtual async Task<PagedResultDto<advPreLogStatusDto>> GetListAsync(advPreLogStatusGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPreLogStatus.advPreLogStatus>, List<advPreLogStatusDto>>(entities);

        return new PagedResultDto<advPreLogStatusDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPreLogStatus
    /// </summary>
    [Authorize(advPreLogStatusPermissions.Create)]
    public virtual async Task<advPreLogStatusDto> CreateAsync(CreateUpdateadvPreLogStatusDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPreLogStatusDto, Sapienza.Lexus.advPreLogStatus.advPreLogStatus>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreLogStatus.advPreLogStatus, advPreLogStatusDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPreLogStatus
    /// </summary>
    [Authorize(advPreLogStatusPermissions.Update)]
    public virtual async Task<advPreLogStatusDto> UpdateAsync(Guid id, CreateUpdateadvPreLogStatusDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPreLogStatus.advPreLogStatus), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreLogStatus.advPreLogStatus, advPreLogStatusDto>(entity);
    }

    /// <summary>
    /// Deletes a advPreLogStatus
    /// </summary>
    [Authorize(advPreLogStatusPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPreLogStatusLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.usuario
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advPreLogStatus.advPreLogStatus> ApplyFilters(IQueryable<Sapienza.Lexus.advPreLogStatus.advPreLogStatus> queryable, advPreLogStatusGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.usuario.Contains(input.Filter))
            .WhereIf(input.idLog != null, x => x.idLog == input.idLog)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.idStatus != null, x => x.idStatus == input.idStatus)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.conversao != null, x => x.conversao == input.conversao)
            .WhereIf(input.tsConversao != null, x => x.tsConversao == input.tsConversao)
            .WhereIf(input.perdido != null, x => x.perdido == input.perdido)
            .WhereIf(input.tsPerdido != null, x => x.tsPerdido == input.tsPerdido)
            .WhereIf(input.diasCorridosDoAnterior != null, x => x.diasCorridosDoAnterior == input.diasCorridosDoAnterior)
            .WhereIf(!input.usuario.IsNullOrWhiteSpace(), x => x.usuario.Contains(input.usuario))
            // ========== FK Filters ==========
            ;
    }
}
