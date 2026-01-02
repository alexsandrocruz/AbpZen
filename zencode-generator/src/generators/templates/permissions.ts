/**
 * Permissions constants template
 */
export function getPermissionsTemplate(): string {
    return `namespace {{ project.namespace }}.Permissions;

public static class {{ project.name }}{{ entity.name }}Permissions
{
    public const string GroupName = "{{ project.name }}";
    
    public const string Default = GroupName + ".{{ entity.name }}";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
}
`;
}

/**
 * Permission definition provider additions
 */
export function getPermissionDefinitionsTemplate(): string {
    return `
        var {{ entity.name | camelCase }}Permission = {{ entity.name | camelCase }}Group.AddPermission({{ project.name }}Permissions.{{ entity.name }}.Default, L("Permission:{{ entity.name }}"));
        {{ entity.name | camelCase }}Permission.AddChild({{ project.name }}Permissions.{{ entity.name }}.Create, L("Permission:{{ entity.name }}.Create"));
        {{ entity.name | camelCase }}Permission.AddChild({{ project.name }}Permissions.{{ entity.name }}.Update, L("Permission:{{ entity.name }}.Update"));
        {{ entity.name | camelCase }}Permission.AddChild({{ project.name }}Permissions.{{ entity.name }}.Delete, L("Permission:{{ entity.name }}.Delete"));
`;
}

/**
 * Localization entries for an entity
 */
export function getLocalizationTemplate(): string {
    return `{
    "Permission:{{ entity.name }}": "{{ entity.name }} management",
    "Permission:{{ entity.name }}.Create": "Create {{ entity.name }}",
    "Permission:{{ entity.name }}.Update": "Edit {{ entity.name }}",
    "Permission:{{ entity.name }}.Delete": "Delete {{ entity.name }}",
    "Menu:{{ entity.name }}": "{{ entity.pluralName }}",
    {%- for field in entity.fields %}
    "{{ entity.name }}:{{ field.name }}": "{{ field.label | default: field.name }}"{% unless forloop.last %},{% endunless %}
    {%- endfor %}
}`;
}
