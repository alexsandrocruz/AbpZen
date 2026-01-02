/**
 * Localization entries for an entity (EN)
 * Returns JSON object with just the text entries to be merged into existing localization file
 */
export function getLocalizationEntriesEnTemplate(): string {
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
    "Successfully{{ entity.name }}Deleted": "{{ entity.name }} deleted successfully."{% for field in entity.fields %},
    "{{ entity.name }}:{{ field.name }}": "{{ field.label | default: field.name }}"{% endfor %}{% for rel in relationships.asChild %},
    "{{ entity.name }}:{{ rel.fkFieldName }}": "{{ rel.parentEntityName }}"{% endfor %}
  }`;
}

/**
 * Localization entries for an entity (PT-BR)
 * Returns JSON object with just the text entries to be merged into existing localization file
 */
export function getLocalizationEntriesPtBrTemplate(): string {
  return `{
    "Permission:{{ entity.name }}": "Gerenciamento de {{ entity.name }}",
    "Permission:{{ entity.name }}.Create": "Criar {{ entity.name }}",
    "Permission:{{ entity.name }}.Update": "Editar {{ entity.name }}",
    "Permission:{{ entity.name }}.Delete": "Excluir {{ entity.name }}",
    "Menu:{{ entity.pluralName }}": "{{ entity.pluralName }}",
    "{{ entity.name }}": "{{ entity.name }}",
    "{{ entity.pluralName }}": "{{ entity.pluralName }}",
    "New{{ entity.name }}": "Novo(a) {{ entity.name }}",
    "Edit{{ entity.name }}": "Editar {{ entity.name }}",
    "{{ entity.name }}DeletionConfirmationMessage": "Tem certeza que deseja excluir este(a) {{ entity.name }}?",
    "Successfully{{ entity.name }}Created": "{{ entity.name }} criado(a) com sucesso.",
    "Successfully{{ entity.name }}Updated": "{{ entity.name }} atualizado(a) com sucesso.",
    "Successfully{{ entity.name }}Deleted": "{{ entity.name }} excluído(a) com sucesso."{% for field in entity.fields %},
    "{{ entity.name }}:{{ field.name }}": "{{ field.label | default: field.name }}"{% endfor %}{% for rel in relationships.asChild %},
    "{{ entity.name }}:{{ rel.fkFieldName }}": "{{ rel.parentEntityName }}"{% endfor %}
  }`;
}

// Keep old functions for backwards compatibility, but deprecated
/** @deprecated Use getLocalizationEntriesEnTemplate instead */
export function getLocalizationJsonTemplate(): string {
  return `{
  "culture": "en",
  "texts": ${getLocalizationEntriesEnTemplate()}
}`;
}

/** @deprecated Use getLocalizationEntriesPtBrTemplate instead */
export function getLocalizationJsonPtBrTemplate(): string {
  return `{
  "culture": "pt-BR",
  "texts": ${getLocalizationEntriesPtBrTemplate()}
}`;
}
