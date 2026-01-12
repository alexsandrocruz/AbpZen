using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.Customer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Customer;

/// <summary>
/// Application service for Customer entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.Customer.Default)]
public class CustomerAppService :
    LeptonXDemoAppAppService,
    ICustomerAppService
{
    private readonly IRepository<LeptonXDemoApp.Customer.Customer, Guid> _repository;

    public CustomerAppService(
        IRepository<LeptonXDemoApp.Customer.Customer, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Customer by Id
    /// </summary>
    public virtual async Task<CustomerDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<LeptonXDemoApp.Customer.Customer, CustomerDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Customers
    /// </summary>
    public virtual async Task<PagedResultDto<CustomerDto>> GetListAsync(CustomerGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.Customer.Customer>, List<CustomerDto>>(entities);

        return new PagedResultDto<CustomerDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Customer
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Customer.Create)]
    public virtual async Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateCustomerDto, LeptonXDemoApp.Customer.Customer>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Customer.Customer, CustomerDto>(entity);
    }

    /// <summary>
    /// Updates an existing Customer
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Customer.Update)]
    public virtual async Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomerDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.Customer.Customer), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Customer.Customer, CustomerDto>(entity);
    }

    /// <summary>
    /// Deletes a Customer
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Customer.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetCustomerLookupAsync()
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
    protected virtual IQueryable<LeptonXDemoApp.Customer.Customer> ApplyFilters(IQueryable<LeptonXDemoApp.Customer.Customer> queryable, CustomerGetListInput input)
    {
        return queryable
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            // ========== FK Filters ==========
            ;
    }
}
