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

    $("#EditalFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#EditalFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/EditalFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('LeptonXDemoApp');
    var createModal = new abp.ModalManager(abp.appPath + 'Edital/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Edital/EditModal');

    var dataTable = $('#EditalTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('LeptonXDemoApp.Edital.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('LeptonXDemoApp.Edital.Delete'),
                            confirmMessage: function (data) {
                                return l('EditalDeletionConfirmationMessage', data.record.id);
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
                title: l('Edital:Objeto'),
                data: "objeto"
            },
            {
                title: l('Edital:Data'),
                data: "data",
                dataFormat: 'datetime'
            },
            {
                title: l('Edital:Valor'),
                data: "valor"
            },
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewEditalButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
