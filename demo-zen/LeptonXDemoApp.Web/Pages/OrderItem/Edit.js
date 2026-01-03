(function ($) {
    var l = abp.localization.getResource('LeptonXDemoApp');
    // Similar logic to Create, but pre-populate generated logic if needed
    // In a real generator, we would fetch existing items via API or existing model
    var _orderList = []; 

    function renderOrdersTable() {
         // ... same rendering logic ...
         var tbody = $('#OrdersTable tbody');
        tbody.empty();

        _orderList.forEach(function (item, index) {
            var row = '<tr>';
            row += '<td>' + (item.productDisplayName || item.name || 'Item ' + (index + 1)) + '</td>';
             row += '<td>' + (item.quantity || 1) + '</td>';
            row += '<td>' + (item.total || 0) + '</td>';
            row += '<td><button type="button" class="btn btn-danger btn-sm delete-order-btn" data-index="' + index + '"><i class="fa fa-trash"></i></button></td>';
            row += '</tr>';
            tbody.append(row);
        });
         $('.delete-order-btn').click(function () {
            var index = $(this).data('index');
            _orderList.splice(index, 1);
            renderOrdersTable();
        });
    }

    abp.modals.EditOrderItem = function () {
        var init = function (modalManager) {
            var $modal = modalManager.getModal();
            var $form = $modal.find('form');
            var id = $form.find('input[name="Id"]').val();

            // Load existing children
            // TODO: In a real app, these might be loaded via AJAX call to GetAsync including navigation properties
            // or embedded in the ViewModel. Assuming ViewModel has them:
            // This part requires the EditModalModel to serialize the list to a JS variable.
            
            // For now, template shell:
             $modal.find('#AddOrderBtn').click(function () {
                var newItem = { id: abp.utils.createGuid() };
                _orderList.push(newItem);
                renderOrdersTable();
            });
            
             $form.on('submit', function (e) {
                 _orderList.forEach(function (item, index) {
                    $form.append('<input type="hidden" name="Orders[' + index + '].id" value="' + (item.id || '') + '" />');
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
