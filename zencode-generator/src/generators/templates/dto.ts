/**
 * DTO template for ABP (Read DTO)
 */
export function getDtoTemplate(): string {
    return `using System;
using Volo.Abp.Application.Dtos;

namespace {{ entity.namespace }}.Dtos;

[Serializable]
public class {{ dto.readTypeName }} : FullAuditedEntityDto<{{ entity.primaryKey }}>
{
    {%- for field in entity.fields %}
    {%- unless field.isLookup %}
    public {{ field.type | csharpType: field.isNullable }} {{ field.name }} { get; set; }
    {%- endunless %}
    {%- endfor %}

    // ========== Foreign Key Fields (1:N Relationships) ==========
    {%- for rel in relationships.asChild %}
    public Guid{% unless rel.isRequired %}?{% endunless %} {{ rel.fkFieldName }} { get; set; }
    public string? {{ rel.parentEntityName }}DisplayName { get; set; }
    {%- endfor %}
}
`;
}

/**
 * CreateUpdate DTO template for ABP
 */
export function getCreateUpdateDtoTemplate(): string {
    return `using System;
using System.ComponentModel.DataAnnotations;

namespace {{ entity.namespace }}.Dtos;

[Serializable]
public class {{ dto.createTypeName }}
{
    {%- for field in entity.fields %}
    {%- unless field.isLookup %}
    {%- if field.isRequired %}
    [Required]
    {%- endif %}
    {%- if field.type == 'string' and field.maxLength %}
    [StringLength({{ field.maxLength }})]
    {%- endif %}
    public {{ field.type | csharpType: field.isNullable }} {{ field.name }} { get; set; }
    {%- endunless %}
    {%- endfor %}

    // ========== Foreign Key Fields (1:N Relationships) ==========
    {%- for rel in relationships.asChild %}
    {%- if rel.isRequired %}
    [Required]
    {%- endif %}
    public Guid{% unless rel.isRequired %}?{% endunless %} {{ rel.fkFieldName }} { get; set; }
    {%- endfor %}
}
`;
}

/**
 * GetListInput template for paged/filtered queries
 */
export function getGetListInputTemplate(): string {
    return `using System;
using Volo.Abp.Application.Dtos;

namespace {{ entity.namespace }}.Dtos;

[Serializable]
public class {{ entity.name }}GetListInput : PagedAndSortedResultRequestDto
{
    {%- for field in entity.fields %}
    {%- if field.isFilterable %}
    {%- if field.type == 'string' %}
    public string? {{ field.name }} { get; set; }
    {%- elsif field.type == 'guid' or field.type == 'int' or field.type == 'long' or field.type == 'datetime' or field.type == 'bool' or field.type == 'decimal' %}
    public {{ field.type | csharpType: true }} {{ field.name }} { get; set; }
    {%- endif %}
    {%- endif %}
    {%- endfor %}

    // ========== FK Filter Fields (Filter by parent entity) ==========
    {%- for rel in relationships.asChild %}
    public Guid? {{ rel.fkFieldName }} { get; set; }
    {%- endfor %}
}
`;
}
