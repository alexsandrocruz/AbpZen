/**
 * Enum class template for ABP Domain.Shared layer
 */
export function getEnumTemplate(): string {
  return `namespace {{ project.namespace }};

/// <summary>
/// {{ enumName }} enum
/// </summary>
public enum {{ enumName }}
{
    {%- for option in options %}
    /// <summary>
    /// {{ option.displayText }}
    /// </summary>
    {{ option.name }} = {{ option.value }}{% unless forloop.last %},{% endunless %}
    {%- endfor %}
}
`;
}

/**
 * Generate localization entries for enum options
 */
export function getEnumLocalizationEnTemplate(): string {
  return `{
  "Enum:{{ enumName }}": "{{ enumName }}",
  {%- for option in options %}
  "Enum:{{ enumName }}.{{ option.name }}": "{{ option.displayText }}"{% unless forloop.last %},{% endunless %}
  {%- endfor %}
}`;
}

export function getEnumLocalizationPtBrTemplate(): string {
  return `{
  "Enum:{{ enumName }}": "{{ enumName }}",
  {%- for option in options %}
  "Enum:{{ enumName }}.{{ option.name }}": "{{ option.displayText }}"{% unless forloop.last %},{% endunless %}
  {%- endfor %}
}`;
}
