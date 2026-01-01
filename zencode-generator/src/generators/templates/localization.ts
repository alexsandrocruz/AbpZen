/**
 * Localization JSON entries for an entity
 */
export function getLocalizationJsonTemplate(): string {
    return `{
  "Permission:{{ entity.name }}": "{{ entity.name }} Management",
  "Permission:{{ entity.name }}.Create": "Create {{ entity.name }}",
  "Permission:{{ entity.name }}.Update": "Edit {{ entity.name }}",
  "Permission:{{ entity.name }}.Delete": "Delete {{ entity.name }}",
  "Menu:{{ entity.pluralName }}": "{{ entity.pluralName }}",
  "{{ entity.name }}": "{{ entity.name }}",
  "{{ entity.pluralName }}": "{{ entity.pluralName }}",
  "New{{ entity.name }}": "New {{ entity.name }}",
  "Edit{{ entity.name }}": "Edit {{ entity.name }}",
  "{{ entity.name }}DeletionConfirmationMessage": "Are you sure you want to delete this {{ entity.name }}?",
  "Successfully{{ entity.name }}Created": "{{ entity.name }} created successfully.",
  "Successfully{{ entity.name }}Updated": "{{ entity.name }} updated successfully.",
  "Successfully{{ entity.name }}Deleted": "{{ entity.name }} deleted successfully."{{ entity.fields | size | if: ',' }}
  {%- for field in entity.fields %}
  "{{ entity.name }}:{{ field.name }}": "{{ field.label | default: field.name }}"{{ unless forloop.last }},{% endunless %}
  {%- endfor %}
}`;
}

/**
 * Localization JSON for Portuguese (pt-BR)
 */
export function getLocalizationJsonPtBrTemplate(): string {
    return `{
  "Permission:{{ entity.name }}": "Gerenciamento de {{ entity.name }}",
  "Permission:{{ entity.name }}.Create": "Criar {{ entity.name }}",
  "Permission:{{ entity.name }}.Update": "Editar {{ entity.name }}",
  "Permission:{{ entity.name }}.Delete": "Excluir {{ entity.name }}",
  "Menu:{{ entity.pluralName }}": "{{ entity.pluralName }}",
  "{{ entity.name }}": "{{ entity.name }}",
  "{{ entity.pluralName }}": "{{ entity.pluralName }}",
  "New{{ entity.name }}": "Novo {{ entity.name }}",
  "Edit{{ entity.name }}": "Editar {{ entity.name }}",
  "{{ entity.name }}DeletionConfirmationMessage": "Tem certeza que deseja excluir este {{ entity.name }}?",
  "Successfully{{ entity.name }}Created": "{{ entity.name }} criado com sucesso.",
  "Successfully{{ entity.name }}Updated": "{{ entity.name }} atualizado com sucesso.",
  "Successfully{{ entity.name }}Deleted": "{{ entity.name }} excluído com sucesso."{{ entity.fields | size | if: ',' }}
  {%- for field in entity.fields %}
  "{{ entity.name }}:{{ field.name }}": "{{ field.label | default: field.name }}"{{ unless forloop.last }},{% endunless %}
  {%- endfor %}
}`;
}
