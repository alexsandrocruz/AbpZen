using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.CustomField.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.CustomField;

/// <summary>
/// Application service for CustomField entity
/// </summary>
[Authorize(CustomFieldPermissions.Default)]
public class CustomFieldAppService :
    DominusAppService,
    ICustomFieldAppService
{
    private readonly IRepository<Dominus.CustomField.CustomField, Guid> _repository;

    public CustomFieldAppService(
        IRepository<Dominus.CustomField.CustomField, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single CustomField by Id
    /// </summary>
    public virtual async Task<CustomFieldDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.CustomField.CustomField, CustomFieldDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of CustomFields
    /// </summary>
    public virtual async Task<PagedResultDto<CustomFieldDto>> GetListAsync(CustomFieldGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.CustomField.CustomField>, List<CustomFieldDto>>(entities);

        return new PagedResultDto<CustomFieldDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new CustomField
    /// </summary>
    [Authorize(CustomFieldPermissions.Create)]
    public virtual async Task<CustomFieldDto> CreateAsync(CreateUpdateCustomFieldDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateCustomFieldDto, Dominus.CustomField.CustomField>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.CustomField.CustomField, CustomFieldDto>(entity);
    }

    /// <summary>
    /// Updates an existing CustomField
    /// </summary>
    [Authorize(CustomFieldPermissions.Update)]
    public virtual async Task<CustomFieldDto> UpdateAsync(Guid id, CreateUpdateCustomFieldDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.CustomField.CustomField), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.CustomField.CustomField, CustomFieldDto>(entity);
    }

    /// <summary>
    /// Deletes a CustomField
    /// </summary>
    [Authorize(CustomFieldPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetCustomFieldLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.EntityType
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.CustomField.CustomField> ApplyFilters(IQueryable<Dominus.CustomField.CustomField> queryable, CustomFieldGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
