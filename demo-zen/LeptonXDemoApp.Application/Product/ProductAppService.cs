using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.Product.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Product;

/// <summary>
/// Application service for Product entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.Product.Default)]
public class ProductAppService :
    LeptonXDemoAppAppService,
    IProductAppService
{
    private readonly IRepository<Product, Guid> _repository;

    public ProductAppService(IRepository<Product, Guid> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Product by Id
    /// </summary>
    public virtual async Task<ProductDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return ObjectMapper.Map<Product, ProductDto>(entity);
    }

    /// <summary>
    /// Gets a paged and filtered list of Products
    /// </summary>
    public virtual async Task<PagedResultDto<ProductDto>> GetListAsync(ProductGetListInput input)
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

        return new PagedResultDto<ProductDto>(
            totalCount,
            ObjectMapper.Map<List<Product>, List<ProductDto>>(entities)
        );
    }

    /// <summary>
    /// Creates a new Product
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Product.Create)]
    public virtual async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProductDto, Product>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Product, ProductDto>(entity);
    }

    /// <summary>
    /// Updates an existing Product
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Product.Update)]
    public virtual async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        var entity = await _repository.GetAsync(id);

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Product, ProductDto>(entity);
    }

    /// <summary>
    /// Deletes a Product
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Product.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Product> ApplyFilters(IQueryable<Product> queryable, ProductGetListInput input)
    {
        return queryable
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            .WhereIf(input.Price != null, x => x.Price == input.Price)
            .WhereIf(input.CategoryId != null, x => x.CategoryId == input.CategoryId)
            // ========== FK Filters ==========
            .WhereIf(input.CategoryId != null, x => x.CategoryId == input.CategoryId)
            ;
    }
}
