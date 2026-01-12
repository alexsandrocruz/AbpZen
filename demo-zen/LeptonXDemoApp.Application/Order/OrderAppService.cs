using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.Order.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Order;

/// <summary>
/// Application service for Order entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.Order.Default)]
public class OrderAppService :
    LeptonXDemoAppAppService,
    IOrderAppService
{
    private readonly IRepository<LeptonXDemoApp.Order.Order, Guid> _repository;
    private readonly IRepository<LeptonXDemoApp.Customer.Customer, Guid> _customerRepository;

    public OrderAppService(
        IRepository<LeptonXDemoApp.Order.Order, Guid> repository,
        IRepository<LeptonXDemoApp.Customer.Customer, Guid> customerRepository
    )
    {
        _repository = repository;
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Gets a single Order by Id
    /// </summary>
    public virtual async Task<OrderDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<LeptonXDemoApp.Order.Order, OrderDto>(entity);
        if (entity.CustomerId != null)
        {
            var parent = await _customerRepository.FindAsync(entity.CustomerId.Value);
            dto.CustomerDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Orders
    /// </summary>
    public virtual async Task<PagedResultDto<OrderDto>> GetListAsync(OrderGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.Order.Order>, List<OrderDto>>(entities);
        var customerIds = entities
            .Where(x => x.CustomerId != null)
            .Select(x => x.CustomerId.Value)
            .Distinct()
            .ToList();

        if (customerIds.Any())
        {
            var parents = await _customerRepository.GetListAsync(x => customerIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.CustomerId != null))
            {
                if (parentMap.TryGetValue(dto.CustomerId.Value, out var displayName))
                {
                    dto.CustomerDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<OrderDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Order
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Order.Create)]
    public virtual async Task<OrderDto> CreateAsync(CreateUpdateOrderDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateOrderDto, LeptonXDemoApp.Order.Order>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Order.Order, OrderDto>(entity);
    }

    /// <summary>
    /// Updates an existing Order
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Order.Update)]
    public virtual async Task<OrderDto> UpdateAsync(Guid id, CreateUpdateOrderDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.Order.Order), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Order.Order, OrderDto>(entity);
    }

    /// <summary>
    /// Deletes a Order
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Order.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetOrderLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Number
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<LeptonXDemoApp.Order.Order> ApplyFilters(IQueryable<LeptonXDemoApp.Order.Order> queryable, OrderGetListInput input)
    {
        return queryable
            .WhereIf(!input.Number.IsNullOrWhiteSpace(), x => x.Number.Contains(input.Number))
            .WhereIf(input.Date != null, x => x.Date == input.Date)
            .WhereIf(input.CustomerId != null, x => x.CustomerId == input.CustomerId)
            // ========== FK Filters ==========
            .WhereIf(input.CustomerId != null, x => x.CustomerId == input.CustomerId)
            ;
    }
}
