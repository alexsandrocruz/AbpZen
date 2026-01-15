using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.BlogCategory.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.BlogCategory;

/// <summary>
/// Application service for BlogCategory entity
/// </summary>
[Authorize(BlogCategoryPermissions.Default)]
public class BlogCategoryAppService :
    DominusAppService,
    IBlogCategoryAppService
{
    private readonly IRepository<Dominus.BlogCategory.BlogCategory, Guid> _repository;

    public BlogCategoryAppService(
        IRepository<Dominus.BlogCategory.BlogCategory, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single BlogCategory by Id
    /// </summary>
    public virtual async Task<BlogCategoryDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.BlogCategory.BlogCategory, BlogCategoryDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of BlogCategories
    /// </summary>
    public virtual async Task<PagedResultDto<BlogCategoryDto>> GetListAsync(BlogCategoryGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.BlogCategory.BlogCategory>, List<BlogCategoryDto>>(entities);

        return new PagedResultDto<BlogCategoryDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new BlogCategory
    /// </summary>
    [Authorize(BlogCategoryPermissions.Create)]
    public virtual async Task<BlogCategoryDto> CreateAsync(CreateUpdateBlogCategoryDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateBlogCategoryDto, Dominus.BlogCategory.BlogCategory>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.BlogCategory.BlogCategory, BlogCategoryDto>(entity);
    }

    /// <summary>
    /// Updates an existing BlogCategory
    /// </summary>
    [Authorize(BlogCategoryPermissions.Update)]
    public virtual async Task<BlogCategoryDto> UpdateAsync(Guid id, CreateUpdateBlogCategoryDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.BlogCategory.BlogCategory), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.BlogCategory.BlogCategory, BlogCategoryDto>(entity);
    }

    /// <summary>
    /// Deletes a BlogCategory
    /// </summary>
    [Authorize(BlogCategoryPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetBlogCategoryLookupAsync()
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
    protected virtual IQueryable<Dominus.BlogCategory.BlogCategory> ApplyFilters(IQueryable<Dominus.BlogCategory.BlogCategory> queryable, BlogCategoryGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
