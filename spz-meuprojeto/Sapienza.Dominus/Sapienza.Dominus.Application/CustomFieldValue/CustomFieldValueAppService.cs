using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.CustomFieldValue.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.CustomFieldValue;

/// <summary>
/// Application service for CustomFieldValue entity
/// </summary>
[Authorize(CustomFieldValuePermissions.Default)]
public class CustomFieldValueAppService :
    DominusAppService,
    ICustomFieldValueAppService
{
    private readonly IRepository<Dominus.CustomFieldValue.CustomFieldValue, Guid> _repository;
    private readonly IRepository<Dominus.CustomField.CustomField, Guid> _customFieldRepository;

    public CustomFieldValueAppService(
        IRepository<Dominus.CustomFieldValue.CustomFieldValue, Guid> repository,
        IRepository<Dominus.CustomField.CustomField, Guid> customFieldRepository
    )
    {
        _repository = repository;
        _customFieldRepository = customFieldRepository;
    }

    /// <summary>
    /// Gets a single CustomFieldValue by Id
    /// </summary>
    public virtual async Task<CustomFieldValueDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.CustomFieldValue.CustomFieldValue, CustomFieldValueDto>(entity);
        if (entity.CustomFieldId != null)
        {
            var parent = await _customFieldRepository.FindAsync(entity.CustomFieldId.Value);
            dto.CustomFieldDisplayName = parent?.EntityType;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of CustomFieldValues
    /// </summary>
    public virtual async Task<PagedResultDto<CustomFieldValueDto>> GetListAsync(CustomFieldValueGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.CustomFieldValue.CustomFieldValue>, List<CustomFieldValueDto>>(entities);
        var customFieldIds = entities
            .Where(x => x.CustomFieldId != null)
            .Select(x => x.CustomFieldId.Value)
            .Distinct()
            .ToList();

        if (customFieldIds.Any())
        {
            var parents = await _customFieldRepository.GetListAsync(x => customFieldIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.EntityType);

            foreach (var dto in dtoList.Where(x => x.CustomFieldId != null))
            {
                if (parentMap.TryGetValue(dto.CustomFieldId.Value, out var displayName))
                {
                    dto.CustomFieldDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<CustomFieldValueDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new CustomFieldValue
    /// </summary>
    [Authorize(CustomFieldValuePermissions.Create)]
    public virtual async Task<CustomFieldValueDto> CreateAsync(CreateUpdateCustomFieldValueDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateCustomFieldValueDto, Dominus.CustomFieldValue.CustomFieldValue>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.CustomFieldValue.CustomFieldValue, CustomFieldValueDto>(entity);
    }

    /// <summary>
    /// Updates an existing CustomFieldValue
    /// </summary>
    [Authorize(CustomFieldValuePermissions.Update)]
    public virtual async Task<CustomFieldValueDto> UpdateAsync(Guid id, CreateUpdateCustomFieldValueDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.CustomFieldValue.CustomFieldValue), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.CustomFieldValue.CustomFieldValue, CustomFieldValueDto>(entity);
    }

    /// <summary>
    /// Deletes a CustomFieldValue
    /// </summary>
    [Authorize(CustomFieldValuePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetCustomFieldValueLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.EntityId
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.CustomFieldValue.CustomFieldValue> ApplyFilters(IQueryable<Dominus.CustomFieldValue.CustomFieldValue> queryable, CustomFieldValueGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.CustomFieldId != null, x => x.CustomFieldId == input.CustomFieldId)
            ;
    }
}
