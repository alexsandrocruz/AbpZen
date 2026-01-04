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

    $("#LeadMessageFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#LeadMessageFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/LeadMessageFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('LeptonXDemoApp');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'LeadMessage/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'LeadMessage/EditModal');

    var dataTable = $('#LeadMessageTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('LeptonXDemoApp.LeadMessage.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('LeptonXDemoApp.LeadMessage.Delete'),
                            confirmMessage: function (data) {
                                return l('LeadMessageDeletionConfirmationMessage', data.record.id);
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
                title: l('LeadMessage:Title'),
                data: "title",
            },
            {
                title: l('LeadMessage:Date'),
                data: "date",
                dataFormat: 'datetime'
            },
            {
                title: l('LeadMessage:Body'),
                data: "body",
            },
            {
                title: l('LeadMessage:Success'),
                data: "success",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('MessageTemplate'),
                data: "messageTemplateDisplayName"
            },
            {
                title: l('Lead'),
                data: "leadDisplayName"
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewLeadMessageButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
