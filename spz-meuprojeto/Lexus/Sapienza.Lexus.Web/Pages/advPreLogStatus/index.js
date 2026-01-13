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

    $("#advPreLogStatusFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advPreLogStatusFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advPreLogStatusFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advPreLogStatus/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advPreLogStatus/EditModal');

    var dataTable = $('#advPreLogStatusTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advPreLogStatus.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advPreLogStatus.Delete'),
                            confirmMessage: function (data) {
                                return l('advPreLogStatusDeletionConfirmationMessage', data.record.id);
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
                title: l('advPreLogStatus:idLog'),
                data: "idLog",
            },
            {
                title: l('advPreLogStatus:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('advPreLogStatus:idStatus'),
                data: "idStatus",
            },
            {
                title: l('advPreLogStatus:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advPreLogStatus:conversao'),
                data: "conversao",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advPreLogStatus:tsConversao'),
                data: "tsConversao",
                dataFormat: 'datetime'
            },
            {
                title: l('advPreLogStatus:perdido'),
                data: "perdido",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advPreLogStatus:tsPerdido'),
                data: "tsPerdido",
                dataFormat: 'datetime'
            },
            {
                title: l('advPreLogStatus:diasCorridosDoAnterior'),
                data: "diasCorridosDoAnterior",
            },
            {
                title: l('advPreLogStatus:usuario'),
                data: "usuario",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvPreLogStatusButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
