using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.FinancialCategory.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.FinancialCategory;

/// <summary>
/// Application service for FinancialCategory entity
/// </summary>
[Authorize(FinancialCategoryPermissions.Default)]
public class FinancialCategoryAppService :
    DominusAppService,
    IFinancialCategoryAppService
{
    private readonly IRepository<Dominus.FinancialCategory.FinancialCategory, Guid> _repository;

    public FinancialCategoryAppService(
        IRepository<Dominus.FinancialCategory.FinancialCategory, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single FinancialCategory by Id
    /// </summary>
    public virtual async Task<FinancialCategoryDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.FinancialCategory.FinancialCategory, FinancialCategoryDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of FinancialCategories
    /// </summary>
    public virtual async Task<PagedResultDto<FinancialCategoryDto>> GetListAsync(FinancialCategoryGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.FinancialCategory.FinancialCategory>, List<FinancialCategoryDto>>(entities);

        return new PagedResultDto<FinancialCategoryDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new FinancialCategory
    /// </summary>
    [Authorize(FinancialCategoryPermissions.Create)]
    public virtual async Task<FinancialCategoryDto> CreateAsync(CreateUpdateFinancialCategoryDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateFinancialCategoryDto, Dominus.FinancialCategory.FinancialCategory>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.FinancialCategory.FinancialCategory, FinancialCategoryDto>(entity);
    }

    /// <summary>
    /// Updates an existing FinancialCategory
    /// </summary>
    [Authorize(FinancialCategoryPermissions.Update)]
    public virtual async Task<FinancialCategoryDto> UpdateAsync(Guid id, CreateUpdateFinancialCategoryDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.FinancialCategory.FinancialCategory), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.FinancialCategory.FinancialCategory, FinancialCategoryDto>(entity);
    }

    /// <summary>
    /// Deletes a FinancialCategory
    /// </summary>
    [Authorize(FinancialCategoryPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetFinancialCategoryLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Name
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.FinancialCategory.FinancialCategory> ApplyFilters(IQueryable<Dominus.FinancialCategory.FinancialCategory> queryable, FinancialCategoryGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
