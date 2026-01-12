$(function () {
    var l = abp.localization.getResource('LeptonXDemoApp');
    var _orderItemList = [];
    var _addOrderItemModal = new bootstrap.Modal(document.getElementById('addOrderItemModal'));

    // Load products into select with Select2
    // Uses ABP lookup endpoint convention: /{entity}/{entity}-lookup
    function initProductSelect() {
        $('#child_ProductId').select2({
            dropdownParent: $('#addOrderItemModal'),
            ajax: {
                url: 'https://localhost:44322/api/app/product/product-lookup',
                dataType: 'json',
                delay: 250,
                xhrFields: {
                    withCredentials: true  // Send authentication cookies
                },
                data: function (params) {
                    return {
                        filter: params.term,
                        maxResultCount: 20
                    };
                },
                processResults: function (data) {
                    // ABP LookupDto format: { items: [{ id, displayName }] }
                    return {
                        results: data.items.map(function (item) {
                            return { id: item.id, text: item.displayName };
                        })
                    };
                },
                cache: true
            },
            placeholder: l('SelectProduct'),
            allowClear: true
        });
    }

    // Calculate total
    function calculateTotal() {
        var quant = parseFloat($('#child_Quant').val()) || 0;
        var price = parseFloat($('#child_Price').val()) || 0;
        var total = quant * price;
        $('#child_Total').val(total.toFixed(2));
    }

    $('#child_Quant, #child_Price').on('input', calculateTotal);

    function renderOrderItemsTable() {
        var tbody = $('#OrderItemsTable tbody');
        tbody.empty();

        _orderItemList.forEach(function (item, index) {
            var row = '<tr>';
            row += '<td>' + (item.productDisplayName || 'Item ' + (index + 1)) + '</td>';
            row += '<td>' + (item.quant || 0) + '</td>';
            row += '<td>' + (item.total || 0).toFixed(2) + '</td>';
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

    // Open modal on Add button click
    $('#AddOrderItemBtn').click(function () {
        // Reset modal fields
        $('#child_ProductId').val(null).trigger('change');
        $('#child_ProductDisplayName').val('');
        $('#child_Quant').val(1);
        $('#child_Price').val('0.00');
        $('#child_Total').val('0.00');

        _addOrderItemModal.show();
    });

    // Confirm button adds item to list
    $('#confirmOrderItemBtn').click(function () {
        var productId = $('#child_ProductId').val();
        var productDisplayName = $('#child_ProductId option:selected').text();
        var quant = parseInt($('#child_Quant').val()) || 0;
        var price = parseFloat($('#child_Price').val()) || 0;
        var total = quant * price;

        if (!productId) {
            abp.message.warn(l('SelectProduct'));
            return;
        }

        if (quant <= 0) {
            abp.message.warn(l('QuantityMustBePositive'));
            return;
        }

        var newItem = {
            id: abp.utils.createGuid(),
            productId: productId,
            productDisplayName: productDisplayName,
            quant: quant,
            price: price,
            total: total
        };

        _orderItemList.push(newItem);
        renderOrderItemsTable();
        _addOrderItemModal.hide();
    });

    // Initialize Select2 after modal is shown
    document.getElementById('addOrderItemModal').addEventListener('shown.bs.modal', function () {
        initProductSelect();
    });

    // Initial Render
    renderOrderItemsTable();

    // Form Submit Injection
    $('#CreateOrderForm').submit(function (e) {
        var $form = $(this);
        _orderItemList.forEach(function (item, index) {
            if (item.productId) {
                $form.append('<input type="hidden" name="OrderItems[' + index + '].ProductId" value="' + item.productId + '" />');
            }
            $form.append('<input type="hidden" name="OrderItems[' + index + '].Quant" value="' + item.quant + '" />');
            $form.append('<input type="hidden" name="OrderItems[' + index + '].Price" value="' + item.price + '" />');
            $form.append('<input type="hidden" name="OrderItems[' + index + '].Total" value="' + item.total + '" />');
        });
        return true;
    });
});
