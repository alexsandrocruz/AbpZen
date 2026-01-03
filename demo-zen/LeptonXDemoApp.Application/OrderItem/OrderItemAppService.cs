using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.OrderItem.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.OrderItem;

/// <summary>
/// Application service for OrderItem entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.OrderItem.Default)]
public class OrderItemAppService :
    LeptonXDemoAppAppService,
    IOrderItemAppService
{
    private readonly IRepository<LeptonXDemoApp.OrderItem.OrderItem, Guid> _repository;
    private readonly IRepository<LeptonXDemoApp.Order.Order, Guid> _orderRepository;

    public OrderItemAppService(
        IRepository<LeptonXDemoApp.OrderItem.OrderItem, Guid> repository,
        IRepository<LeptonXDemoApp.Order.Order, Guid> orderRepository
    )
    {
        _repository = repository;
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Gets a single OrderItem by Id
    /// </summary>
    public virtual async Task<OrderItemDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<LeptonXDemoApp.OrderItem.OrderItem, OrderItemDto>(entity);
        if (entity.OrderId != null)
        {
            var parent = await _orderRepository.FindAsync(entity.OrderId.Value);
            dto.OrderDisplayName = parent?.Number;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of OrderItems
    /// </summary>
    public virtual async Task<PagedResultDto<OrderItemDto>> GetListAsync(OrderItemGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.OrderItem.OrderItem>, List<OrderItemDto>>(entities);
        var orderIds = entities
            .Where(x => x.OrderId != null)
            .Select(x => x.OrderId.Value)
            .Distinct()
            .ToList();

        if (orderIds.Any())
        {
            var parents = await _orderRepository.GetListAsync(x => orderIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Number);

            foreach (var dto in dtoList.Where(x => x.OrderId != null))
            {
                if (parentMap.TryGetValue(dto.OrderId.Value, out var displayName))
                {
                    dto.OrderDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<OrderItemDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new OrderItem
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.OrderItem.Create)]
    public virtual async Task<OrderItemDto> CreateAsync(CreateUpdateOrderItemDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateOrderItemDto, LeptonXDemoApp.OrderItem.OrderItem>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.OrderItem.OrderItem, OrderItemDto>(entity);
    }

    /// <summary>
    /// Updates an existing OrderItem
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.OrderItem.Update)]
    public virtual async Task<OrderItemDto> UpdateAsync(Guid id, CreateUpdateOrderItemDto input)
    {
        // Fetch with details for Master-Detail update
        var query = await _repository.WithDetailsAsync(x => x.);
        var entity = await AsyncExecuter.FirstOrDefaultAsync(query, x => x.Id == id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.OrderItem.OrderItem), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.OrderItem.OrderItem, OrderItemDto>(entity);
    }

    /// <summary>
    /// Deletes a OrderItem
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.OrderItem.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetOrderItemLookupAsync()
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
    protected virtual IQueryable<LeptonXDemoApp.OrderItem.OrderItem> ApplyFilters(IQueryable<LeptonXDemoApp.OrderItem.OrderItem> queryable, OrderItemGetListInput input)
    {
        return queryable
            .WhereIf(input.ProductId != null, x => x.ProductId == input.ProductId)
            // ========== FK Filters ==========
            .WhereIf(input.OrderId != null, x => x.OrderId == input.OrderId)
            ;
    }
}
