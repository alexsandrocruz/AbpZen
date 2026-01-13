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

    $("#fabPermissoesFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#fabPermissoesFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/fabPermissoesFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'fabPermissoes/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'fabPermissoes/EditModal');

    var dataTable = $('#fabPermissoesTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.fabPermissoes.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.fabPermissoes.Delete'),
                            confirmMessage: function (data) {
                                return l('fabPermissoesDeletionConfirmationMessage', data.record.id);
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
                title: l('fabPermissoes:idPermissao'),
                data: "idPermissao",
            },
            {
                title: l('fabPermissoes:idPermissaoTipo'),
                data: "idPermissaoTipo",
            },
            {
                title: l('fabPermissoes:modulo'),
                data: "modulo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabPermissoes:descricao'),
                data: "descricao",
            },
            {
                title: l('fabPermissoes:varSession'),
                data: "varSession",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewfabPermissoesButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
