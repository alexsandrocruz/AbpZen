/**
 * AppService template for ABP (following ABP Tutorial pattern with explicit [Authorize] attributes)
 */
export function getAppServiceTemplate(): string {
    return `using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using {{ project.namespace }}.Permissions;
using {{ entity.namespace }}.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace {{ entity.namespace }};

/// <summary>
/// Application service for {{ entity.name }} entity
/// </summary>
[Authorize({{ project.name }}Permissions.{{ entity.name }}.Default)]
public class {{ entity.name }}AppService :
    {{ project.name }}AppService,
    I{{ entity.name }}AppService
{
    private readonly IRepository<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}, {{ entity.primaryKey }}> _repository;

    public {{ entity.name }}AppService(IRepository<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}, {{ entity.primaryKey }}> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single {{ entity.name }} by Id
    /// </summary>
    public virtual async Task<{{ dto.readTypeName }}> GetAsync({{ entity.primaryKey }} id)
    {
        var entity = await _repository.GetAsync(id);
        return ObjectMapper.Map<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}, {{ dto.readTypeName }}>(entity);
    }

    /// <summary>
    /// Gets a paged and filtered list of {{ entity.pluralName }}
    /// </summary>
    public virtual async Task<PagedResultDto<{{ dto.readTypeName }}>> GetListAsync({{ entity.name }}GetListInput input)
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

        return new PagedResultDto<{{ dto.readTypeName }}>(
            totalCount,
            ObjectMapper.Map<List<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}>, List<{{ dto.readTypeName }}>>(entities)
        );
    }

    /// <summary>
    /// Creates a new {{ entity.name }}
    /// </summary>
    [Authorize({{ project.name }}Permissions.{{ entity.name }}.Create)]
    public virtual async Task<{{ dto.readTypeName }}> CreateAsync({{ dto.createTypeName }} input)
    {
        var entity = ObjectMapper.Map<{{ dto.createTypeName }}, {{ project.namespace }}.{{ entity.name }}.{{ entity.name }}>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}, {{ dto.readTypeName }}>(entity);
    }

    /// <summary>
    /// Updates an existing {{ entity.name }}
    /// </summary>
    [Authorize({{ project.name }}Permissions.{{ entity.name }}.Update)]
    public virtual async Task<{{ dto.readTypeName }}> UpdateAsync({{ entity.primaryKey }} id, {{ dto.updateTypeName }} input)
    {
        var entity = await _repository.GetAsync(id);

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}, {{ dto.readTypeName }}>(entity);
    }

    /// <summary>
    /// Deletes a {{ entity.name }}
    /// </summary>
    [Authorize({{ project.name }}Permissions.{{ entity.name }}.Delete)]
    public virtual async Task DeleteAsync({{ entity.primaryKey }} id)
    {
        await _repository.DeleteAsync(id);
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}> ApplyFilters(IQueryable<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}> queryable, {{ entity.name }}GetListInput input)
    {
        return queryable
            {%- for field in entity.fields %}
            {%- if field.isFilterable %}
            {%- if field.type == 'string' %}
            .WhereIf(!input.{{ field.name }}.IsNullOrWhiteSpace(), x => x.{{ field.name }}.Contains(input.{{ field.name }}))
            {%- else %}
            .WhereIf(input.{{ field.name }} != null, x => x.{{ field.name }} == input.{{ field.name }})
            {%- endif %}
            {%- endif %}
            {%- endfor %}
            // ========== FK Filters ==========
            {%- for rel in relationships.asChild %}
            .WhereIf(input.{{ rel.fkFieldName }} != null, x => x.{{ rel.fkFieldName }} == input.{{ rel.fkFieldName }})
            {%- endfor %}
            ;
    }
}
`;
}

/**
 * AppService interface template for ABP
 */
export function getAppServiceInterfaceTemplate(): string {
    return `using System;
using {{ entity.namespace }}.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace {{ entity.namespace }};

public interface I{{ entity.name }}AppService :
    ICrudAppService<
        {{ dto.readTypeName }},
        {{ entity.primaryKey }},
        {{ entity.name }}GetListInput,
        {{ dto.createTypeName }},
        {{ dto.updateTypeName }}>
{
}
`;
}
