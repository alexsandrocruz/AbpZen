using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPreCheckLists.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreCheckLists;

/// <summary>
/// Application service for advPreCheckLists entity
/// </summary>
[Authorize(advPreCheckListsPermissions.Default)]
public class advPreCheckListsAppService :
    LexusAppService,
    IadvPreCheckListsAppService
{
    private readonly IRepository<Sapienza.Lexus.advPreCheckLists.advPreCheckLists, Guid> _repository;

    public advPreCheckListsAppService(
        IRepository<Sapienza.Lexus.advPreCheckLists.advPreCheckLists, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPreCheckLists by Id
    /// </summary>
    public virtual async Task<advPreCheckListsDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPreCheckLists.advPreCheckLists, advPreCheckListsDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPreCheckListses
    /// </summary>
    public virtual async Task<PagedResultDto<advPreCheckListsDto>> GetListAsync(advPreCheckListsGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPreCheckLists.advPreCheckLists>, List<advPreCheckListsDto>>(entities);

        return new PagedResultDto<advPreCheckListsDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPreCheckLists
    /// </summary>
    [Authorize(advPreCheckListsPermissions.Create)]
    public virtual async Task<advPreCheckListsDto> CreateAsync(CreateUpdateadvPreCheckListsDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPreCheckListsDto, Sapienza.Lexus.advPreCheckLists.advPreCheckLists>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreCheckLists.advPreCheckLists, advPreCheckListsDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPreCheckLists
    /// </summary>
    [Authorize(advPreCheckListsPermissions.Update)]
    public virtual async Task<advPreCheckListsDto> UpdateAsync(Guid id, CreateUpdateadvPreCheckListsDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPreCheckLists.advPreCheckLists), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreCheckLists.advPreCheckLists, advPreCheckListsDto>(entity);
    }

    /// <summary>
    /// Deletes a advPreCheckLists
    /// </summary>
    [Authorize(advPreCheckListsPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPreCheckListsLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advPreCheckLists.advPreCheckLists> ApplyFilters(IQueryable<Sapienza.Lexus.advPreCheckLists.advPreCheckLists> queryable, advPreCheckListsGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idCheckList != null, x => x.idCheckList == input.idCheckList)
            .WhereIf(input.idGrupo != null, x => x.idGrupo == input.idGrupo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
