/**
 * Entity class template for ABP Domain layer
 */
export function getEntityTemplate(): string {
    return `using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace {{ project.namespace }}.{{ entity.name }};

/// <summary>
/// {{ entity.name }} entity
/// </summary>
public class {{ entity.name }} : {{ entity.baseClass }}<{{ entity.primaryKey }}>
{
    {%- for field in entity.fields %}
    {%- unless field.isLookup %}
    {%- assign isFk = false %}
    {%- for rel in relationships.asChild %}
      {%- if rel.fkFieldName == field.name %}{% assign isFk = true %}{% endif %}
    {%- endfor %}
    {%- unless isFk %}
    {%- if field.type == 'string' %}
    public string{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }{% if field.isRequired %} = string.Empty;{% endif %}

    {%- elsif field.type == 'guid' %}
    public Guid{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'int' %}
    public int{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'long' %}
    public long{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'decimal' %}
    public decimal{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'bool' %}
    public bool{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'datetime' %}
    public DateTime{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- else %}
    public {{ field.type }}{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- endif %}
    {%- endunless %}
    {%- endunless %}
    {%- endfor %}

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    {%- for rel in relationships.asChild %}
    public Guid{% unless rel.isRequired %}?{% endunless %} {{ rel.fkFieldName }} { get; set; }
    {%- endfor %}

    // ========== Navigation Properties ==========
    {%- for rel in relationships.asChild %}
    public virtual {{ project.namespace }}.{{ rel.parentEntityName }}.{{ rel.parentEntityName }}{% unless rel.isRequired %}?{% endunless %} {{ rel.navigationName }} { get; set; }
    {%- endfor %}

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    {%- for rel in relationships.asParent %}
    public virtual ICollection<{{ project.namespace }}.{{ rel.childEntityName }}.{{ rel.childEntityName }}> {{ rel.navigationName }} { get; set; } = new List<{{ project.namespace }}.{{ rel.childEntityName }}.{{ rel.childEntityName }}>();
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
