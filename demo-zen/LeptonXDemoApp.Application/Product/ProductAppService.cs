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
    private readonly IRepository<LeptonXDemoApp.Product.Product, Guid> _repository;
    private readonly IRepository<LeptonXDemoApp.Category.Category, Guid> _categoryRepository;

    public ProductAppService(
        IRepository<LeptonXDemoApp.Product.Product, Guid> repository,
        IRepository<LeptonXDemoApp.Category.Category, Guid> categoryRepository
    )
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// Gets a single Product by Id
    /// </summary>
    public virtual async Task<ProductDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<LeptonXDemoApp.Product.Product, ProductDto>(entity);
        if (entity.CategoryId != null)
        {
            var parent = await _categoryRepository.FindAsync(entity.CategoryId.Value);
            dto.CategoryDisplayName = parent?.Name;
        }

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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.Product.Product>, List<ProductDto>>(entities);
        var categoryIds = entities
            .Where(x => x.CategoryId != null)
            .Select(x => x.CategoryId.Value)
            .Distinct()
            .ToList();

        if (categoryIds.Any())
        {
            var parents = await _categoryRepository.GetListAsync(x => categoryIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.CategoryId != null))
            {
                if (parentMap.TryGetValue(dto.CategoryId.Value, out var displayName))
                {
                    dto.CategoryDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<ProductDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Product
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Product.Create)]
    public virtual async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProductDto, LeptonXDemoApp.Product.Product>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Product.Product, ProductDto>(entity);
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

        return ObjectMapper.Map<LeptonXDemoApp.Product.Product, ProductDto>(entity);
    }

    /// <summary>
    /// Deletes a Product
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Product.Delete)]
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
    protected virtual IQueryable<LeptonXDemoApp.Product.Product> ApplyFilters(IQueryable<LeptonXDemoApp.Product.Product> queryable, ProductGetListInput input)
    {
        return queryable
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            .WhereIf(!input.Price.IsNullOrWhiteSpace(), x => x.Price.Contains(input.Price))
            .WhereIf(input.CategoryId != null, x => x.CategoryId == input.CategoryId)
            // ========== FK Filters ==========
            .WhereIf(input.CategoryId != null, x => x.CategoryId == input.CategoryId)
            ;
    }
}
