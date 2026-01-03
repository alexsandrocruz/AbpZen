$(function () {

    function debounce(func, delay) {
        let timerId;
        return function (...args) {
            clearTimeout(timerId);
            timerId = setTimeout(() => {
                func.apply(this, args);
            }, delay);
        };
    }

    $("#OrderFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#OrderFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/OrderFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('LeptonXDemoApp');

    // Master-Detail: Full Page CRUD (OrderItems child grid)

    var dataTable = $('#OrderTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('LeptonXDemoApp.Order.Update'),
                            action: function (data) {
                                // Full Page navigation
                                window.location.href = '/Order/Edit/' + data.record.id;
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('LeptonXDemoApp.Order.Delete'),
                            confirmMessage: function (data) {
                                return l('OrderDeletionConfirmationMessage', data.record.id);
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
                title: l('Order:Number'),
                data: "number",
            },
            {
                title: l('Order:Date'),
                data: "date",
                dataFormat: 'datetime'
            },
            {
                title: l('Customer'),
                data: "customerDisplayName"
            },
        ]
    }));

    $('#NewOrderButton').click(function (e) {
        e.preventDefault();
        // Full Page navigation
        window.location.href = '/Order/Create';
    });
});

