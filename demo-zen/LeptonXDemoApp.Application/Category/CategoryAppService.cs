using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.Category.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Category;

/// <summary>
/// Application service for Category entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.Category.Default)]
public class CategoryAppService :
    LeptonXDemoAppAppService,
    ICategoryAppService
{
    private readonly IRepository<Category, Guid> _repository;

    public CategoryAppService(IRepository<Category, Guid> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Category by Id
    /// </summary>
    public virtual async Task<CategoryDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return ObjectMapper.Map<Category, CategoryDto>(entity);
    }

    /// <summary>
    /// Gets a paged and filtered list of Categories
    /// </summary>
    public virtual async Task<PagedResultDto<CategoryDto>> GetListAsync(CategoryGetListInput input)
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

        return new PagedResultDto<CategoryDto>(
            totalCount,
            ObjectMapper.Map<List<Category>, List<CategoryDto>>(entities)
        );
    }

    /// <summary>
    /// Creates a new Category
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Category.Create)]
    public virtual async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateCategoryDto, Category>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Category, CategoryDto>(entity);
    }

    /// <summary>
    /// Updates an existing Category
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Category.Update)]
    public virtual async Task<CategoryDto> UpdateAsync(Guid id, CreateUpdateCategoryDto input)
    {
        var entity = await _repository.GetAsync(id);

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Category, CategoryDto>(entity);
    }

    /// <summary>
    /// Deletes a Category
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Category.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Category> ApplyFilters(IQueryable<Category> queryable, CategoryGetListInput input)
    {
        return queryable
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            // ========== FK Filters ==========
            ;
    }
}
