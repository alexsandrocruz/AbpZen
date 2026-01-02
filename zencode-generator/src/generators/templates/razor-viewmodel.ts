/**
 * Create ViewModel template for Razor Pages
 */
export function getRazorCreateViewModelTemplate(): string {
    return `using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace {{ project.namespace }}.Web.Pages.{{ entity.name }}.ViewModels;

public class Create{{ entity.name }}ViewModel
{
    {%- for field in entity.fields %}
    {%- unless field.isLookup %}
    {%- assign isFk = false %}
    {%- for rel in relationships.asChild %}
      {%- if rel.fkFieldName == field.name %}{% assign isFk = true %}{% endif %}
    {%- endfor %}
    {%- unless isFk %}
    {%- if field.isRequired %}
    [Required]
    {%- endif %}
    {%- if field.type == 'string' and field.maxLength %}
    [StringLength({{ field.maxLength }})]
    {%- endif %}
    [Display(Name = "{{ entity.name }}:{{ field.name }}")]
    {%- if field.isTextArea %}
    [TextArea(Rows = 3)]
    {%- endif %}
    {%- if field.type == 'string' %}
    public string{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }{% if field.isRequired %} = string.Empty;{% endif %}

    {%- elsif field.type == 'int' %}
    public int{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'guid' %}
    public Guid{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'datetime' %}
    public DateTime{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'bool' %}
    public bool{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'decimal' %}
    public decimal{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'enum' and field.enumConfig %}
    {%- if field.isNullable %}
    public {{ field.enumConfig.enumName }}? {{ field.name }} { get; set; }
    {%- else %}
    public {{ field.enumConfig.enumName }} {{ field.name }} { get; set; }
    {%- endif %}
    {%- else %}
    public {{ field.type | csharpType: field.isNullable }} {{ field.name }} { get; set; }
    {%- endif %}
    {%- endunless %}
    {%- endunless %}
    {%- endfor %}

    // ========== Foreign Key Fields (1:N Relationships) ==========
    {%- for rel in relationships.asChild %}
    {%- if rel.isRequired %}
    [Required]
    {%- endif %}
    [Display(Name = "{{ entity.name }}:{{ rel.fkFieldName }}")]
    [SelectItems(nameof({{ rel.parentEntityName }}List))]
    public Guid{% unless rel.isRequired %}?{% endunless %} {{ rel.fkFieldName }} { get; set; }

    public List<SelectListItem> {{ rel.parentEntityName }}List { get; set; } = new();
    {%- endfor %}
}
`;
}

/**
 * Edit ViewModel template for Razor Pages
 */
export function getRazorEditViewModelTemplate(): string {
    return `using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace {{ project.namespace }}.Web.Pages.{{ entity.name }}.ViewModels;

public class Edit{{ entity.name }}ViewModel
{
    {%- for field in entity.fields %}
    {%- unless field.isLookup %}
    {%- assign isFk = false %}
    {%- for rel in relationships.asChild %}
      {%- if rel.fkFieldName == field.name %}{% assign isFk = true %}{% endif %}
    {%- endfor %}
    {%- unless isFk %}
    {%- if field.isRequired %}
    [Required]
    {%- endif %}
    {%- if field.type == 'string' and field.maxLength %}
    [StringLength({{ field.maxLength }})]
    {%- endif %}
    [Display(Name = "{{ entity.name }}:{{ field.name }}")]
    {%- if field.isTextArea %}
    [TextArea(Rows = 3)]
    {%- endif %}
    {%- if field.readOnly %}
    [ReadOnlyInput]
    {%- endif %}
    {%- if field.type == 'string' %}
    public string{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }{% if field.isRequired %} = string.Empty;{% endif %}

    {%- elsif field.type == 'int' %}
    public int{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'guid' %}
    public Guid{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'datetime' %}
    public DateTime{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'bool' %}
    public bool{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'decimal' %}
    public decimal{% if field.isNullable %}?{% endif %} {{ field.name }} { get; set; }
    {%- elsif field.type == 'enum' and field.enumConfig %}
    {%- if field.isNullable %}
    public {{ field.enumConfig.enumName }}? {{ field.name }} { get; set; }
    {%- else %}
    public {{ field.enumConfig.enumName }} {{ field.name }} { get; set; }
    {%- endif %}
    {%- else %}
    public {{ field.type | csharpType: field.isNullable }} {{ field.name }} { get; set; }
    {%- endif %}
    {%- endunless %}
    {%- endunless %}
    {%- endfor %}

    // ========== Foreign Key Fields (1:N Relationships) ==========
    {%- for rel in relationships.asChild %}
    {%- if rel.isRequired %}
    [Required]
    {%- endif %}
    [Display(Name = "{{ entity.name }}:{{ rel.fkFieldName }}")]
    [SelectItems(nameof({{ rel.parentEntityName }}List))]
    public Guid{% unless rel.isRequired %}?{% endunless %} {{ rel.fkFieldName }} { get; set; }

    public List<SelectListItem> {{ rel.parentEntityName }}List { get; set; } = new();
    {%- endfor %}
}
`;
}

/**
 * Web AutoMapper profile additions for ViewModels
 */
export function getRazorAutoMapperProfileTemplate(): string {
    return `using AutoMapper;
using {{ project.namespace }}.{{ entity.name }}.Dtos;
using {{ project.namespace }}.Web.Pages.{{ entity.name }}.ViewModels;

namespace {{ project.namespace }}.Web;

public class {{ entity.name }}WebAutoMapperProfile : Profile
{
    public {{ entity.name }}WebAutoMapperProfile()
    {
        CreateMap<{{ dto.readTypeName }}, Edit{{ entity.name }}ViewModel>();
        CreateMap<Create{{ entity.name }}ViewModel, {{ dto.createTypeName }}>();
        CreateMap<Edit{{ entity.name }}ViewModel, {{ dto.updateTypeName }}>();
    }
}
`;
}
