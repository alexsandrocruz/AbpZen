
export function getRazorCreateModalJsTemplate(): string {
    return `(function ($) {
    var l = abp.localization.getResource('{{ project.name }}');
    {%- for rel in relationships.asParent %}
    {%- if rel.isChildGrid %}
    var _{{ rel.targetEntityName | camelCase }}List = [];
    var _{{ rel.targetEntityName | camelCase }}Table = null;

    function render{{ rel.targetPluralName }}Table() {
        var tbody = $('#{{ rel.targetPluralName }}Table tbody');
        tbody.empty();

        _{{ rel.targetEntityName | camelCase }}List.forEach(function (item, index) {
            var row = '<tr>';
            // TODO: Dynamic columns based on Child Entity Fields
            row += '<td>' + (item.productDisplayName || item.name || 'Item ' + (index + 1)) + '</td>';
            row += '<td>' + (item.quantity || 1) + '</td>';
            row += '<td>' + (item.total || 0) + '</td>';
            row += '<td><button type="button" class="btn btn-danger btn-sm delete-{{ rel.targetEntityName | camelCase }}-btn" data-index="' + index + '"><i class="fa fa-trash"></i></button></td>';
            row += '</tr>';
            tbody.append(row);
        });

        // Re-bind delete events
        $('.delete-{{ rel.targetEntityName | camelCase }}-btn').click(function () {
            var index = $(this).data('index');
            _{{ rel.targetEntityName | camelCase }}List.splice(index, 1);
            render{{ rel.targetPluralName }}Table();
        });
    }

    abp.modals.Create{{ entity.name }} = function () {
        var init = function (modalManager) {
            var $modal = modalManager.getModal();
            var $form = $modal.find('form');

            // Add Item Handler
            $modal.find('#Add{{ rel.targetEntityName }}Btn').click(function () {
                // TODO: Open a cleaner sub-modal or inline row
                // For now, mock adding an item
                var newItem = {
                    id: abp.utils.createGuid(), // Temp ID
                    // TODO: meaningful defaults
                };
                _{{ rel.targetEntityName | camelCase }}List.push(newItem);
                render{{ rel.targetPluralName }}Table();
            });

            // Hook into Form Submit
            $form.on('submit', function (e) {
                // Serialize the list into hidden fields or rely on JSON body override
                // ABP form submit usually serializes the form. We need to inject our list.
                // A common trick is to append hidden inputs with array index notation:
                // Items[0].Id, Items[0].Quantity...
                
                _{{ rel.targetEntityName | camelCase }}List.forEach(function (item, index) {
                    $form.append('<input type="hidden" name="{{ rel.targetPluralName }}[' + index + '].id" value="' + (item.id || '') + '" />');
                    // Iterate other props
                    for (var prop in item) {
                         $form.append('<input type="hidden" name="{{ rel.targetPluralName }}[' + index + '].' + prop + '" value="' + item[prop] + '" />');
                    }
                });
            });
        };

        return {
            init: init
        };
    };
    {%- endif %}
    {%- endfor %}
})(jQuery);
`;
}

export function getRazorEditModalJsTemplate(): string {
    return `(function ($) {
    var l = abp.localization.getResource('{{ project.name }}');
    {%- for rel in relationships.asParent %}
    {%- if rel.isChildGrid %}
    // Similar logic to Create, but pre-populate generated logic if needed
    // In a real generator, we would fetch existing items via API or existing model
    var _{{ rel.targetEntityName | camelCase }}List = []; 

    function render{{ rel.targetPluralName }}Table() {
         // ... same rendering logic ...
         var tbody = $('#{{ rel.targetPluralName }}Table tbody');
        tbody.empty();

        _{{ rel.targetEntityName | camelCase }}List.forEach(function (item, index) {
            var row = '<tr>';
            row += '<td>' + (item.productDisplayName || item.name || 'Item ' + (index + 1)) + '</td>';
             row += '<td>' + (item.quantity || 1) + '</td>';
            row += '<td>' + (item.total || 0) + '</td>';
            row += '<td><button type="button" class="btn btn-danger btn-sm delete-{{ rel.targetEntityName | camelCase }}-btn" data-index="' + index + '"><i class="fa fa-trash"></i></button></td>';
            row += '</tr>';
            tbody.append(row);
        });
         $('.delete-{{ rel.targetEntityName | camelCase }}-btn').click(function () {
            var index = $(this).data('index');
            _{{ rel.targetEntityName | camelCase }}List.splice(index, 1);
            render{{ rel.targetPluralName }}Table();
        });
    }

    abp.modals.Edit{{ entity.name }} = function () {
        var init = function (modalManager) {
            var $modal = modalManager.getModal();
            var $form = $modal.find('form');
            var id = $form.find('input[name="Id"]').val();

            // Load existing children
            // TODO: In a real app, these might be loaded via AJAX call to GetAsync including navigation properties
            // or embedded in the ViewModel. Assuming ViewModel has them:
            // This part requires the EditModalModel to serialize the list to a JS variable.
            
            // For now, template shell:
             $modal.find('#Add{{ rel.targetEntityName }}Btn').click(function () {
                var newItem = { id: abp.utils.createGuid() };
                _{{ rel.targetEntityName | camelCase }}List.push(newItem);
                render{{ rel.targetPluralName }}Table();
            });
            
             $form.on('submit', function (e) {
                 _{{ rel.targetEntityName | camelCase }}List.forEach(function (item, index) {
                    $form.append('<input type="hidden" name="{{ rel.targetPluralName }}[' + index + '].id" value="' + (item.id || '') + '" />');
                     for (var prop in item) {
                         $form.append('<input type="hidden" name="{{ rel.targetPluralName }}[' + index + '].' + prop + '" value="' + item[prop] + '" />');
                    }
                });
            });
        };

        return {
            init: init
        };
    };
    {%- endif %}
    {%- endfor %}
})(jQuery);
`;
}
