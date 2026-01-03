$(function () {
    var l = abp.localization.getResource('LeptonXDemoApp');
    var _orderItemList = [];

    // Load initial data
    if (typeof orderItemInitialData !== 'undefined') {
        _orderItemList = orderItemInitialData;
    }

    function renderOrderItemsTable() {
        var tbody = $('#OrderItemsTable tbody');
        tbody.empty();

        _orderItemList.forEach(function (item, index) {
            var row = '<tr>';
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
        var newItem = {
            id: abp.utils.createGuid(),
            quantity: 1,
            total: 100
        };
        _orderItemList.push(newItem);
        renderOrderItemsTable();
    });
    
    renderOrderItemsTable();

    $('#EditOrderForm').submit(function (e) {
        var $form = $(this);
        _orderItemList.forEach(function (item, index) {
            if(item.id) {
                $form.append('<input type="hidden" name="OrderItems[' + index + '].id" value="' + item.id + '" />');
            }
            for (var prop in item) {
                if(prop !== 'id' && typeof item[prop] !== 'function' && typeof item[prop] !== 'object') {
                     $form.append('<input type="hidden" name="OrderItems[' + index + '].' + prop + '" value="' + item[prop] + '" />');
                }
            }
        });
        return true;
    });
});
