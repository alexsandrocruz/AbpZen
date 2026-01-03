$(function () {
    var l = abp.localization.getResource('LeptonXDemoApp');
    var _orderItemList = [];
    
    function renderOrderItemsTable() {
        var tbody = $('#OrderItemsTable tbody');
        tbody.empty();

        _orderItemList.forEach(function (item, index) {
            var row = '<tr>';
            // TODO: Dynamic columns based on Child Entity Fields
            row += '<td>' + (item.productDisplayName || item.name || 'Item ' + (index + 1)) + '</td>';
            row += '<td>' + (item.quantity || 1) + '</td>';
            row += '<td>' + (item.total || 0) + '</td>';
            row += '<td><button type="button" class="btn btn-danger btn-sm delete-orderItem-btn" data-index="' + index + '"><i class="fa fa-trash"></i></button></td>';
            row += '</tr>';
            tbody.append(row);
        });

        $('.delete-orderItem-btn').click(function () {
            var index = $(this).data('index');
            _orderItemList.splice(index, 1);
            renderOrderItemsTable();
        });
    }

    $('#AddOrderItemBtn').click(function () {
        // Mock Add Logic
        var newItem = {
            id: abp.utils.createGuid(),
            quantity: 1,
            total: 100
        };
        _orderItemList.push(newItem);
        renderOrderItemsTable();
    });
    
    // Initial Render
    renderOrderItemsTable();
    
    // Form Submit Injection
    $('#CreateOrderForm').submit(function (e) {
        var $form = $(this);
        _orderItemList.forEach(function (item, index) {
            // In C#, Binding a List<T> requires indexers: OrderItems[0].Id
            // We iterate properties of the item
            
            // Core ID (if exists, though create usually doesn't have valid ID for child yet, but FKs do)
            if(item.id) {
                $form.append('<input type="hidden" name="OrderItems[' + index + '].id" value="' + item.id + '" />');
            }

            // Iterate other props (Mocked for now, assumes 'item' is flat object matching DTO)
            for (var prop in item) {
                if(prop !== 'id' && typeof item[prop] !== 'function' && typeof item[prop] !== 'object') {
                     $form.append('<input type="hidden" name="OrderItems[' + index + '].' + prop + '" value="' + item[prop] + '" />');
                }
            }
        });
        // Allow form to continue
        return true;
    });
});
