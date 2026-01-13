using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus._versaoBD.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus._versaoBD;

/// <summary>
/// Application service for _versaoBD entity
/// </summary>
[Authorize(_versaoBDPermissions.Default)]
public class _versaoBDAppService :
    LexusAppService,
    I_versaoBDAppService
{
    private readonly IRepository<Sapienza.Lexus._versaoBD._versaoBD, Guid> _repository;

    public _versaoBDAppService(
        IRepository<Sapienza.Lexus._versaoBD._versaoBD, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single _versaoBD by Id
    /// </summary>
    public virtual async Task<_versaoBDDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus._versaoBD._versaoBD, _versaoBDDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of _versaoBDs
    /// </summary>
    public virtual async Task<PagedResultDto<_versaoBDDto>> GetListAsync(_versaoBDGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus._versaoBD._versaoBD>, List<_versaoBDDto>>(entities);

        return new PagedResultDto<_versaoBDDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new _versaoBD
    /// </summary>
    [Authorize(_versaoBDPermissions.Create)]
    public virtual async Task<_versaoBDDto> CreateAsync(CreateUpdate_versaoBDDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdate_versaoBDDto, Sapienza.Lexus._versaoBD._versaoBD>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus._versaoBD._versaoBD, _versaoBDDto>(entity);
    }

    /// <summary>
    /// Updates an existing _versaoBD
    /// </summary>
    [Authorize(_versaoBDPermissions.Update)]
    public virtual async Task<_versaoBDDto> UpdateAsync(Guid id, CreateUpdate_versaoBDDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus._versaoBD._versaoBD), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus._versaoBD._versaoBD, _versaoBDDto>(entity);
    }

    /// <summary>
    /// Deletes a _versaoBD
    /// </summary>
    [Authorize(_versaoBDPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> Get_versaoBDLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.arquivo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus._versaoBD._versaoBD> ApplyFilters(IQueryable<Sapienza.Lexus._versaoBD._versaoBD> queryable, _versaoBDGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.arquivo.Contains(input.Filter))
            .WhereIf(input.id != null, x => x.id == input.id)
            .WhereIf(!input.arquivo.IsNullOrWhiteSpace(), x => x.arquivo.Contains(input.arquivo))
            .WhereIf(input.dataAplicacao != null, x => x.dataAplicacao == input.dataAplicacao)
            // ========== FK Filters ==========
            ;
    }
}
