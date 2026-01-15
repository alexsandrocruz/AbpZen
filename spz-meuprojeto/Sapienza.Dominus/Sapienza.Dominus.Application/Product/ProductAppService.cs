using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Product.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Product;

/// <summary>
/// Application service for Product entity
/// </summary>
[Authorize(ProductPermissions.Default)]
public class ProductAppService :
    DominusAppService,
    IProductAppService
{
    private readonly IRepository<Dominus.Product.Product, Guid> _repository;

    public ProductAppService(
        IRepository<Dominus.Product.Product, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Product by Id
    /// </summary>
    public virtual async Task<ProductDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Product.Product, ProductDto>(entity);

        return dto;
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
        var dtoList = ObjectMapper.Map<List<Dominus.Product.Product>, List<ProductDto>>(entities);

        return new PagedResultDto<ProductDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Product
    /// </summary>
    [Authorize(ProductPermissions.Create)]
    public virtual async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProductDto, Dominus.Product.Product>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Product.Product, ProductDto>(entity);
    }

    /// <summary>
    /// Updates an existing Product
    /// </summary>
    [Authorize(ProductPermissions.Update)]
    public virtual async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Product.Product), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Product.Product, ProductDto>(entity);
    }

    /// <summary>
    /// Deletes a Product
    /// </summary>
    [Authorize(ProductPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProductLookupAsync()
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
    protected virtual IQueryable<Dominus.Product.Product> ApplyFilters(IQueryable<Dominus.Product.Product> queryable, ProductGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
