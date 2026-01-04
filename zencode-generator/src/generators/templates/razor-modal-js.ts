
export function getRazorCreateModalJsTemplate(): string {
    return `(function ($) {
    var l = abp.localization.getResource('{{ project.name }}');

    abp.modals.Create{{ entity.name }} = function () {
        var init = function (modalManager) {
            var $modal = modalManager.getModal();
            var $form = $modal.find('form');

            {%- for rel in relationships.asParent -%}
            {%- if rel.isChildGrid %}
            // ---------- {{ rel.targetPluralName }} Child Grid ----------
            var _{{ rel.targetPluralName | camelCase }}Table = $('#{{ rel.targetPluralName }}Table').DataTable(
                abp.libs.datatables.normalizeConfiguration({
                    processing: true,
                    serverSide: false,
                    searching: false,
                    paging: false,
                    info: false,
                    scrollX: true,
                    columnDefs: [
                        {
                            title: l('Actions'),
                            rowAction: {
                                items: [
                                    {
                                        text: l('Delete'),
                                        action: function (data) {
                                            _{{ rel.targetPluralName | camelCase }}Table.row(data.record.row).remove().draw();
                                        }
                                    }
                                ]
                            }
                        },
                        {%- for field in rel.targetFields -%}
                        {%- if field.name != 'Id' and field.name != 'TenantId' and field.isLookup == false -%}
                        {
                            title: l('{{ rel.targetEntityName }}:{{ field.name }}'),
                            data: "{{ field.name | camelCase }}"
                        },
                        {%- elsif field.isLookup -%}
                        {
                            title: l('{{ rel.targetEntityName }}:{{ field.name }}'),
                            data: "{{ field.lookupConfig.targetEntity | camelCase }}DisplayName",
                            defaultContent: ""
                        },
                        {%- endif -%}
                        {%- endfor %}
                    ]
                })
            );

            $modal.find('#Add{{ rel.targetEntityName }}Btn').click(function () {
                {%- if rel.isManyToMany -%}
                // Many-to-Many: Open Lookup Modal for Target Entity
                // For now, placeholder prompt
                var name = prompt("Select {{ rel.targetPluralName }} (Placeholder)");
                if (name) {
                    _{{ rel.targetPluralName | camelCase }}Table.row.add({
                        id: abp.utils.createGuid(),
                        {{ rel.displayField | camelCase }}: name,
                        // Add other junction fields with defaults
                        {%- for field in rel.targetFields -%}
                        {%- if field.isLookup == false and field.name != 'Id' and field.name != 'TenantId' -%}
                        {{ field.name | camelCase }}: null,
                        {%- endif -%}
                        {%- endfor %}
                    }).draw();
                }
                {%- else -%}
                // One-to-Many: Open Create Sub-Modal
                // TODO: Implement sub-modal
                _{{ rel.targetPluralName | camelCase }}Table.row.add({ id: abp.utils.createGuid() }).draw();
                {%- endif -%}
            });

            // Sync with form submission
            $form.on('submit', function (e) {
                var data = _{{ rel.targetPluralName | camelCase }}Table.data().toArray();
                data.forEach(function (item, index) {
                    {%- for field in rel.targetFields -%}
                    {%- if field.name != 'TenantId' -%}
                    $form.append('<input type="hidden" name="ViewModel.{{ rel.targetPluralName }}[' + index + '].{{ field.name }}" value="' + (item.{{ field.name | camelCase }} || '') + '" />');
                    {%- endif -%}
                    {%- endfor %}
                });
            });
            {%- endif %}
            {%- endfor %}

            // Adjust columns on tab change
            $modal.find('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            });
        };

        return {
            init: init
        };
    };
})(jQuery);
`;
}

export function getRazorEditModalJsTemplate(): string {
    return `(function ($) {
    var l = abp.localization.getResource('{{ project.name }}');

    abp.modals.Edit{{ entity.name }} = function () {
        var init = function (modalManager) {
            var $modal = modalManager.getModal();
            var $form = $modal.find('form');

            {%- for rel in relationships.asParent -%}
            {%- if rel.isChildGrid %}
            // ---------- {{ rel.targetPluralName }} Child Grid ----------
            var _{{ rel.targetPluralName | camelCase }}Table = $('#{{ rel.targetPluralName }}Table').DataTable(
                abp.libs.datatables.normalizeConfiguration({
                    processing: true,
                    serverSide: false,
                    searching: false,
                    paging: false,
                    info: false,
                    scrollX: true,
                    columnDefs: [
                        {
                            title: l('Actions'),
                            rowAction: {
                                items: [
                                    {
                                        text: l('Delete'),
                                        action: function (data) {
                                            _{{ rel.targetPluralName | camelCase }}Table.row(data.record.row).remove().draw();
                                        }
                                    }
                                ]
                            }
                        },
                        {%- for field in rel.targetFields -%}
                        {%- if field.name != 'Id' and field.name != 'TenantId' and field.isLookup == false -%}
                        {
                            title: l('{{ rel.targetEntityName }}:{{ field.name }}'),
                            data: "{{ field.name | camelCase }}"
                        },
                        {%- elsif field.isLookup -%}
                        {
                            title: l('{{ rel.targetEntityName }}:{{ field.name }}'),
                            data: "{{ field.lookupConfig.targetEntity | camelCase }}DisplayName",
                            defaultContent: ""
                        },
                        {%- endif -%}
                        {%- endfor %}
                    ]
                })
            );

            // Load existing data? 
            // In Edit modal, child items should probably be part of the ViewModel and serialized to JS.
            // TODO: Pre-populate _{{ rel.targetPluralName | camelCase }}Table with initial data

            $modal.find('#Add{{ rel.targetEntityName }}Btn').click(function () {
                {%- if rel.isManyToMany -%}
                var name = prompt("Select {{ rel.targetPluralName }} (Placeholder)");
                if (name) {
                    _{{ rel.targetPluralName | camelCase }}Table.row.add({
                        id: abp.utils.createGuid(),
                        {{ rel.displayField | camelCase }}: name,
                         {%- for field in rel.targetFields -%}
                        {%- if field.isLookup == false and field.name != 'Id' and field.name != 'TenantId' -%}
                        {{ field.name | camelCase }}: null,
                        {%- endif -%}
                        {%- endfor %}
                    }).draw();
                }
                {%- else -%}
                _{{ rel.targetPluralName | camelCase }}Table.row.add({ id: abp.utils.createGuid() }).draw();
                {%- endif -%}
            });

            $form.on('submit', function (e) {
                var data = _{{ rel.targetPluralName | camelCase }}Table.data().toArray();
                data.forEach(function (item, index) {
                    {%- for field in rel.targetFields -%}
                    {%- if field.name != 'TenantId' -%}
                    $form.append('<input type="hidden" name="ViewModel.{{ rel.targetPluralName }}[' + index + '].{{ field.name }}" value="' + (item.{{ field.name | camelCase }} || '') + '" />');
                    {%- endif -%}
                    {%- endfor %}
                });
            });
            {%- endif %}
            {%- endfor %}

            $modal.find('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            });
        };

        return {
            init: init
        };
    };
})(jQuery);
`;
}
