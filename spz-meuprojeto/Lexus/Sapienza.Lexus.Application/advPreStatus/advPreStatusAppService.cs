using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPreStatus.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreStatus;

/// <summary>
/// Application service for advPreStatus entity
/// </summary>
[Authorize(advPreStatusPermissions.Default)]
public class advPreStatusAppService :
    LexusAppService,
    IadvPreStatusAppService
{
    private readonly IRepository<Sapienza.Lexus.advPreStatus.advPreStatus, Guid> _repository;

    public advPreStatusAppService(
        IRepository<Sapienza.Lexus.advPreStatus.advPreStatus, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPreStatus by Id
    /// </summary>
    public virtual async Task<advPreStatusDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPreStatus.advPreStatus, advPreStatusDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPreStatuses
    /// </summary>
    public virtual async Task<PagedResultDto<advPreStatusDto>> GetListAsync(advPreStatusGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPreStatus.advPreStatus>, List<advPreStatusDto>>(entities);

        return new PagedResultDto<advPreStatusDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPreStatus
    /// </summary>
    [Authorize(advPreStatusPermissions.Create)]
    public virtual async Task<advPreStatusDto> CreateAsync(CreateUpdateadvPreStatusDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPreStatusDto, Sapienza.Lexus.advPreStatus.advPreStatus>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreStatus.advPreStatus, advPreStatusDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPreStatus
    /// </summary>
    [Authorize(advPreStatusPermissions.Update)]
    public virtual async Task<advPreStatusDto> UpdateAsync(Guid id, CreateUpdateadvPreStatusDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPreStatus.advPreStatus), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreStatus.advPreStatus, advPreStatusDto>(entity);
    }

    /// <summary>
    /// Deletes a advPreStatus
    /// </summary>
    [Authorize(advPreStatusPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPreStatusLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advPreStatus.advPreStatus> ApplyFilters(IQueryable<Sapienza.Lexus.advPreStatus.advPreStatus> queryable, advPreStatusGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idStatus != null, x => x.idStatus == input.idStatus)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.ordem != null, x => x.ordem == input.ordem)
            .WhereIf(input.ultimo != null, x => x.ultimo == input.ultimo)
            .WhereIf(input.diasMaxParado != null, x => x.diasMaxParado == input.diasMaxParado)
            .WhereIf(input.idTipo != null, x => x.idTipo == input.idTipo)
            // ========== FK Filters ==========
            ;
    }
}
