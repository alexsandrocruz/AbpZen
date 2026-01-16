using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPreProcessosCheckLists.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreProcessosCheckLists;

/// <summary>
/// Application service for advPreProcessosCheckLists entity
/// </summary>
[Authorize(advPreProcessosCheckListsPermissions.Default)]
public class advPreProcessosCheckListsAppService :
    LexusAppService,
    IadvPreProcessosCheckListsAppService
{
    private readonly IRepository<Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists, Guid> _repository;

    public advPreProcessosCheckListsAppService(
        IRepository<Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPreProcessosCheckLists by Id
    /// </summary>
    public virtual async Task<advPreProcessosCheckListsDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists, advPreProcessosCheckListsDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPreProcessosCheckListses
    /// </summary>
    public virtual async Task<PagedResultDto<advPreProcessosCheckListsDto>> GetListAsync(advPreProcessosCheckListsGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists>, List<advPreProcessosCheckListsDto>>(entities);

        return new PagedResultDto<advPreProcessosCheckListsDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPreProcessosCheckLists
    /// </summary>
    [Authorize(advPreProcessosCheckListsPermissions.Create)]
    public virtual async Task<advPreProcessosCheckListsDto> CreateAsync(CreateUpdateadvPreProcessosCheckListsDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPreProcessosCheckListsDto, Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists, advPreProcessosCheckListsDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPreProcessosCheckLists
    /// </summary>
    [Authorize(advPreProcessosCheckListsPermissions.Update)]
    public virtual async Task<advPreProcessosCheckListsDto> UpdateAsync(Guid id, CreateUpdateadvPreProcessosCheckListsDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists, advPreProcessosCheckListsDto>(entity);
    }

    /// <summary>
    /// Deletes a advPreProcessosCheckLists
    /// </summary>
    [Authorize(advPreProcessosCheckListsPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPreProcessosCheckListsLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.grupo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists> ApplyFilters(IQueryable<Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists> queryable, advPreProcessosCheckListsGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.grupo.Contains(input.Filter) || x.item.Contains(input.Filter) || x.tsConclusao.Contains(input.Filter))
            .WhereIf(input.idPreCheckList != null, x => x.idPreCheckList == input.idPreCheckList)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.idGrupo != null, x => x.idGrupo == input.idGrupo)
            .WhereIf(input.idCheckList != null, x => x.idCheckList == input.idCheckList)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(!input.grupo.IsNullOrWhiteSpace(), x => x.grupo.Contains(input.grupo))
            .WhereIf(!input.item.IsNullOrWhiteSpace(), x => x.item.Contains(input.item))
            .WhereIf(input.concluido != null, x => x.concluido == input.concluido)
            .WhereIf(!input.tsConclusao.IsNullOrWhiteSpace(), x => x.tsConclusao.Contains(input.tsConclusao))
            .WhereIf(input.ResponsibleUserId != null, x => x.ResponsibleUserId == input.ResponsibleUserId)
            .WhereIf(input.ordem != null, x => x.ordem == input.ordem)
            // ========== FK Filters ==========
            ;
    }
}
