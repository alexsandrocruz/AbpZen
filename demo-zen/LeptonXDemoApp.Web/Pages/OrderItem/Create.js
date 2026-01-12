(function ($) {
    var l = abp.localization.getResource('LeptonXDemoApp');
    var _orderList = [];
    var _orderTable = null;

    function renderOrdersTable() {
        var tbody = $('#OrdersTable tbody');
        tbody.empty();

        _orderList.forEach(function (item, index) {
            var row = '<tr>';
            // TODO: Dynamic columns based on Child Entity Fields
            row += '<td>' + (item.productDisplayName || item.name || 'Item ' + (index + 1)) + '</td>';
            row += '<td>' + (item.quantity || 1) + '</td>';
            row += '<td>' + (item.total || 0) + '</td>';
            row += '<td><button type="button" class="btn btn-danger btn-sm delete-order-btn" data-index="' + index + '"><i class="fa fa-trash"></i></button></td>';
            row += '</tr>';
            tbody.append(row);
        });

        // Re-bind delete events
        $('.delete-order-btn').click(function () {
            var index = $(this).data('index');
            _orderList.splice(index, 1);
            renderOrdersTable();
        });
    }

    abp.modals.CreateOrderItem = function () {
        var init = function (modalManager) {
            var $modal = modalManager.getModal();
            var $form = $modal.find('form');

            // Add Item Handler
            $modal.find('#AddOrderBtn').click(function () {
                // TODO: Open a cleaner sub-modal or inline row
                // For now, mock adding an item
                var newItem = {
                    id: abp.utils.createGuid(), // Temp ID
                    // TODO: meaningful defaults
                };
                _orderList.push(newItem);
                renderOrdersTable();
            });

            // Hook into Form Submit
            $form.on('submit', function (e) {
                // Serialize the list into hidden fields or rely on JSON body override
                // ABP form submit usually serializes the form. We need to inject our list.
                // A common trick is to append hidden inputs with array index notation:
                // Items[0].Id, Items[0].Quantity...
                
                _orderList.forEach(function (item, index) {
                    $form.append('<input type="hidden" name="Orders[' + index + '].id" value="' + (item.id || '') + '" />');
                    // Iterate other props
                    for (var prop in item) {
                         $form.append('<input type="hidden" name="Orders[' + index + '].' + prop + '" value="' + item[prop] + '" />');
                    }
                });
            });
        };

        return {
            init: init
        };
    };
})(jQuery);
