using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPreOrigens.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreOrigens;

/// <summary>
/// Application service for advPreOrigens entity
/// </summary>
[Authorize(advPreOrigensPermissions.Default)]
public class advPreOrigensAppService :
    LexusAppService,
    IadvPreOrigensAppService
{
    private readonly IRepository<Sapienza.Lexus.advPreOrigens.advPreOrigens, Guid> _repository;

    public advPreOrigensAppService(
        IRepository<Sapienza.Lexus.advPreOrigens.advPreOrigens, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPreOrigens by Id
    /// </summary>
    public virtual async Task<advPreOrigensDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPreOrigens.advPreOrigens, advPreOrigensDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPreOrigenses
    /// </summary>
    public virtual async Task<PagedResultDto<advPreOrigensDto>> GetListAsync(advPreOrigensGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPreOrigens.advPreOrigens>, List<advPreOrigensDto>>(entities);

        return new PagedResultDto<advPreOrigensDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPreOrigens
    /// </summary>
    [Authorize(advPreOrigensPermissions.Create)]
    public virtual async Task<advPreOrigensDto> CreateAsync(CreateUpdateadvPreOrigensDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPreOrigensDto, Sapienza.Lexus.advPreOrigens.advPreOrigens>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreOrigens.advPreOrigens, advPreOrigensDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPreOrigens
    /// </summary>
    [Authorize(advPreOrigensPermissions.Update)]
    public virtual async Task<advPreOrigensDto> UpdateAsync(Guid id, CreateUpdateadvPreOrigensDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPreOrigens.advPreOrigens), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreOrigens.advPreOrigens, advPreOrigensDto>(entity);
    }

    /// <summary>
    /// Deletes a advPreOrigens
    /// </summary>
    [Authorize(advPreOrigensPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPreOrigensLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advPreOrigens.advPreOrigens> ApplyFilters(IQueryable<Sapienza.Lexus.advPreOrigens.advPreOrigens> queryable, advPreOrigensGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idOrigem != null, x => x.idOrigem == input.idOrigem)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
