/**
 * Permissions constants template
 */
export function getPermissionsTemplate(): string {
    return `namespace {{ project.namespace }}.Permissions;

public static class {{ entity.name }}Permissions
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
        var {{ entity.name | camelCase }}Permission = myGroup.AddPermission({{ entity.name }}Permissions.Default, L("Permission:{{ entity.name }}"));
        {{ entity.name | camelCase }}Permission.AddChild({{ entity.name }}Permissions.Create, L("Permission:Create"));
        {{ entity.name | camelCase }}Permission.AddChild({{ entity.name }}Permissions.Update, L("Permission:Update"));
        {{ entity.name | camelCase }}Permission.AddChild({{ entity.name }}Permissions.Delete, L("Permission:Delete"));
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
