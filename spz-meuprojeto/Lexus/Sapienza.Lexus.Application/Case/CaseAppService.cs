using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.Case.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.Case;

/// <summary>
/// Application service for Case entity
/// </summary>
[Authorize(CasePermissions.Default)]
public class CaseAppService :
    LexusAppService,
    ICaseAppService
{
    private readonly IRepository<Sapienza.Lexus.Case.Case, Guid> _repository;

    public CaseAppService(
        IRepository<Sapienza.Lexus.Case.Case, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Case by Id
    /// </summary>
    public virtual async Task<CaseDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.Case.Case, CaseDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Cases
    /// </summary>
    public virtual async Task<PagedResultDto<CaseDto>> GetListAsync(CaseGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.Case.Case>, List<CaseDto>>(entities);

        return new PagedResultDto<CaseDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Case
    /// </summary>
    [Authorize(CasePermissions.Create)]
    public virtual async Task<CaseDto> CreateAsync(CreateUpdateCaseDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateCaseDto, Sapienza.Lexus.Case.Case>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.Case.Case, CaseDto>(entity);
    }

    /// <summary>
    /// Updates an existing Case
    /// </summary>
    [Authorize(CasePermissions.Update)]
    public virtual async Task<CaseDto> UpdateAsync(Guid id, CreateUpdateCaseDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.Case.Case), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.Case.Case, CaseDto>(entity);
    }

    /// <summary>
    /// Deletes a Case
    /// </summary>
    [Authorize(CasePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetCaseLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Title
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.Case.Case> ApplyFilters(IQueryable<Sapienza.Lexus.Case.Case> queryable, CaseGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.CaseNumber.Contains(input.Filter) || x.Title.Contains(input.Filter))
            .WhereIf(!input.CaseNumber.IsNullOrWhiteSpace(), x => x.CaseNumber.Contains(input.CaseNumber))
            .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title.Contains(input.Title))
            // ========== FK Filters ==========
            ;
    }
}
