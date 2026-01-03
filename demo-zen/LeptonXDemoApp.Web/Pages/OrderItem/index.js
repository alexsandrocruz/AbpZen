$(function () {

    function debounce(func, delay) {
        let timerId;
        return function(...args) {
            clearTimeout(timerId);
            timerId = setTimeout(() => {
                func.apply(this, args);
            }, delay);
        };
    }

    $("#OrderItemFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#OrderItemFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/OrderItemFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('LeptonXDemoApp');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'OrderItem/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'OrderItem/EditModal');

    var dataTable = $('#OrderItemTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(function (input) {
            return abp.ajax({
                url: '?handler=List',
                type: 'GET',
                data: input
            });
        }, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items: [
                        {
                            text: l('Edit'),
                            visible: abp.auth.isGranted('LeptonXDemoApp.OrderItem.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('LeptonXDemoApp.OrderItem.Delete'),
                            confirmMessage: function (data) {
                                return l('OrderItemDeletionConfirmationMessage', data.record.id);
                            },
                            action: function (data) {
                                abp.ajax({
                                    url: '?handler=Delete&id=' + data.record.id,
                                    type: 'POST'
                                })
                                .then(function () {
                                    abp.notify.info(l('SuccessfullyDeleted'));
                                    dataTable.ajax.reload(null, false);
                                });
                            }
                        }
                    ]
                }
            },
            {
                title: l('OrderItem:OrderId'),
                data: "orderId",
            },
            {
                title: l('OrderItem:Quant'),
                data: "quant",
            },
            {
                title: l('OrderItem:Price'),
                data: "price",
            },
            {
                title: l('OrderItem:Total'),
                data: "total",
            },
            {
                title: l('Order'),
                data: "orderDisplayName"
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewOrderItemButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
