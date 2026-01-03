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
    {%- for rel in relationships.asChild %}
    private readonly IRepository<{{ project.namespace }}.{{ rel.parentEntityName }}.{{ rel.parentEntityName }}, Guid> _{{ rel.parentEntityName | camelCase }}Repository;
    {%- endfor %}

    public {{ entity.name }}AppService(
        IRepository<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}, {{ entity.primaryKey }}> repository{% if relationships.asChild.size > 0 %},{% endif %}
        {%- for rel in relationships.asChild %}
        IRepository<{{ project.namespace }}.{{ rel.parentEntityName }}.{{ rel.parentEntityName }}, Guid> {{ rel.parentEntityName | camelCase }}Repository{% unless forloop.last %},{% endunless %}
        {%- endfor %}
    )
    {
        _repository = repository;
        {%- for rel in relationships.asChild %}
        _{{ rel.parentEntityName | camelCase }}Repository = {{ rel.parentEntityName | camelCase }}Repository;
        {%- endfor %}
    }

    /// <summary>
    /// Gets a single {{ entity.name }} by Id
    /// </summary>
    public virtual async Task<{{ dto.readTypeName }}> GetAsync({{ entity.primaryKey }} id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}, {{ dto.readTypeName }}>(entity);

        {%- for rel in relationships.asChild %}
        if (entity.{{ rel.fkFieldName }} != null)
        {
            var parent = await _{{ rel.parentEntityName | camelCase }}Repository.FindAsync(entity.{{ rel.fkFieldName }}.Value);
            dto.{{ rel.parentEntityName }}DisplayName = parent?.{{ rel.displayField }};
        }
        {%- endfor %}

        return dto;
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
        var dtoList = ObjectMapper.Map<List<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}>, List<{{ dto.readTypeName }}>>(entities);

        {%- for rel in relationships.asChild %}
        var {{ rel.parentEntityName | camelCase }}Ids = entities
            .Where(x => x.{{ rel.fkFieldName }} != null)
            .Select(x => x.{{ rel.fkFieldName }}.Value)
            .Distinct()
            .ToList();

        if ({{ rel.parentEntityName | camelCase }}Ids.Any())
        {
            var parents = await _{{ rel.parentEntityName | camelCase }}Repository.GetListAsync(x => {{ rel.parentEntityName | camelCase }}Ids.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.{{ rel.displayField }});

            foreach (var dto in dtoList.Where(x => x.{{ rel.fkFieldName }} != null))
            {
                if (parentMap.TryGetValue(dto.{{ rel.fkFieldName }}.Value, out var displayName))
                {
                    dto.{{ rel.parentEntityName }}DisplayName = displayName;
                }
            }
        }
        {%- endfor %}

        return new PagedResultDto<{{ dto.readTypeName }}>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new {{ entity.name }}
    /// </summary>
    [Authorize({{ project.name }}Permissions.{{ entity.name }}.Create)]
    public virtual async Task<{{ dto.readTypeName }}> CreateAsync({{ dto.createTypeName }} input)
    {
        var entity = ObjectMapper.Map<{{ dto.createTypeName }}, {{ project.namespace }}.{{ entity.name }}.{{ entity.name }}>(input);

        {%- for rel in relationships.asParent %}
        {%- if rel.isChildGrid %}
        // Master-Detail: {{ rel.targetEntityName }}
        if (input.{{ rel.targetPluralName }} != null && input.{{ rel.targetPluralName }}.Any())
        {
            foreach (var itemDto in input.{{ rel.targetPluralName }})
            {
                var item = ObjectMapper.Map<CreateUpdate{{ rel.targetEntityName }}Dto, {{ project.namespace }}.{{ rel.targetEntityName }}.{{ rel.targetEntityName }}>(itemDto);
                entity.{{ rel.targetPluralName }}.Add(item);
            }
        }
        {%- endif %}
        {%- endfor %}

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<{{ project.namespace }}.{{ entity.name }}.{{ entity.name }}, {{ dto.readTypeName }}>(entity);
    }

    /// <summary>
    /// Updates an existing {{ entity.name }}
    /// </summary>
    [Authorize({{ project.name }}Permissions.{{ entity.name }}.Update)]
    public virtual async Task<{{ dto.readTypeName }}> UpdateAsync({{ entity.primaryKey }} id, {{ dto.updateTypeName }} input)
    {
        {%- assign hasChildGrids = false -%}
        {%- for rel in relationships.asParent -%}
            {%- if rel.isChildGrid -%}{%- assign hasChildGrids = true -%}{%- break -%}{%- endif -%}
        {%- endfor -%}
        {%- if hasChildGrids %}
        // Fetch with details for Master-Detail update
        var query = await _repository.WithDetailsAsync(x => x.{% for rel in relationships.asParent %}{% if rel.isChildGrid %}{{ rel.targetPluralName }}{% endif %}{% endfor %});
        var entity = await AsyncExecuter.FirstOrDefaultAsync(query, x => x.Id == id);
        {%- else %}
        var entity = await _repository.GetAsync(id);
        {%- endif %}
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof({{ project.namespace }}.{{ entity.name }}.{{ entity.name }}), id);
        }

        ObjectMapper.Map(input, entity);

        {%- for rel in relationships.asParent %}
        {%- if rel.isChildGrid %}
        // Master-Detail Reconciliation: {{ rel.targetEntityName }}
        if (input.{{ rel.targetPluralName }} != null)
        {
            // 1. Remove deleted items
            var inputIds = input.{{ rel.targetPluralName }}.Select(x => x.Id).Where(x => x != Guid.Empty).ToList();
            var itemsToRemove = entity.{{ rel.targetPluralName }}.Where(x => !inputIds.Contains(x.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                entity.{{ rel.targetPluralName }}.Remove(item);
            }

            // 2. Add or Update
            foreach (var itemDto in input.{{ rel.targetPluralName }})
            {
                if (itemDto.Id == Guid.Empty)
                {
                    // Add new
                    var newItem = ObjectMapper.Map<CreateUpdate{{ rel.targetEntityName }}Dto, {{ project.namespace }}.{{ rel.targetEntityName }}.{{ rel.targetEntityName }}>(itemDto);
                    entity.{{ rel.targetPluralName }}.Add(newItem);
                }
                else
                {
                    // Update existing
                    var existingItem = entity.{{ rel.targetPluralName }}.FirstOrDefault(x => x.Id == itemDto.Id);
                    if (existingItem != null)
                    {
                        ObjectMapper.Map(itemDto, existingItem);
                    }
                }
            }
        }
        {%- endif %}
        {%- endfor %}

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

    public virtual async Task<ListResultDto<LookupDto<{{ entity.primaryKey }}>>> Get{{ entity.name }}LookupAsync()
    {
        var entities = await _repository.GetListAsync();
        {%- assign displayField = "" -%}
        {%- assign foundDisplay = false -%}
        {%- for field in entity.fields -%}
            {%- if field.name == "Name" or field.name == "name" -%}{%- assign displayField = field.name -%}{%- assign foundDisplay = true -%}{%- break -%}{%- endif -%}
        {%- endfor -%}
        {%- unless foundDisplay -%}
            {%- for field in entity.fields -%}
                {%- if field.name == "Title" or field.name == "title" -%}{%- assign displayField = field.name -%}{%- assign foundDisplay = true -%}{%- break -%}{%- endif -%}
            {%- endfor -%}
        {%- endunless -%}
        {%- unless foundDisplay -%}
            {%- for field in entity.fields -%}
                {%- if field.type == "string" -%}{%- assign displayField = field.name -%}{%- assign foundDisplay = true -%}{%- break -%}{%- endif -%}
            {%- endfor -%}
        {%- endunless -%}
        return new ListResultDto<LookupDto<{{ entity.primaryKey }}>>(
            entities.Select(x => new LookupDto<{{ entity.primaryKey }}>
            {
                Id = x.Id,
                {%- if foundDisplay %}
                DisplayName = x.{{ displayField }}
                {%- else %}
                DisplayName = x.Id.ToString()
                {%- endif %}
            }).ToList()
        );
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
using System.Threading.Tasks;
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
    Task<ListResultDto<LookupDto<{{ entity.primaryKey }}>>> Get{{ entity.name }}LookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
`;
}
