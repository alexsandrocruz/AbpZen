/**
 * Entity class template for ABP Domain layer
 */
export function getEntityTemplate(): string {
    return `using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace {{ project.namespace }}.{{ entity.name }};

/// <summary>
/// {{ entity.name }} entity
/// </summary>
public class {{ entity.name }} : {{ entity.baseClass }}<{{ entity.primaryKey }}>
{
    {%- for field in entity.fields %}
    {%- unless field.isLookup %}
    {%- if field.type == 'string' %}
    public string{{ field.isNullable | if: '?' }} {{ field.name }} { get; set; }{{ field.isRequired | if: ' = string.Empty;' }}
    {%- elsif field.type == 'guid' %}
    public Guid{{ field.isNullable | if: '?' }} {{ field.name }} { get; set; }
    {%- elsif field.type == 'int' %}
    public int{{ field.isNullable | if: '?' }} {{ field.name }} { get; set; }
    {%- elsif field.type == 'long' %}
    public long{{ field.isNullable | if: '?' }} {{ field.name }} { get; set; }
    {%- elsif field.type == 'decimal' %}
    public decimal{{ field.isNullable | if: '?' }} {{ field.name }} { get; set; }
    {%- elsif field.type == 'bool' %}
    public bool{{ field.isNullable | if: '?' }} {{ field.name }} { get; set; }
    {%- elsif field.type == 'datetime' %}
    public DateTime{{ field.isNullable | if: '?' }} {{ field.name }} { get; set; }
    {%- else %}
    public {{ field.type }}{{ field.isNullable | if: '?' }} {{ field.name }} { get; set; }
    {%- endif %}
    {%- endunless %}
    {%- endfor %}

    protected {{ entity.name }}()
    {
        // Required by EF Core
    }

    public {{ entity.name }}({{ entity.primaryKey }} id) : base(id)
    {
    }
}
`;
}

/**
 * Entity Consts class template (for string lengths, etc.)
 */
export function getEntityConstsTemplate(): string {
    return `namespace {{ project.namespace }}.{{ entity.name }};

public static class {{ entity.name }}Consts
{
    {%- for field in entity.fields %}
    {%- if field.type == 'string' and field.maxLength %}
    public const int Max{{ field.name }}Length = {{ field.maxLength }};
    {%- endif %}
    {%- endfor %}
}
`;
}
