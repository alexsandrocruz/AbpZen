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

    $("#finExtratoFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#finExtratoFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/finExtratoFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'finExtrato/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'finExtrato/EditModal');

    var dataTable = $('#finExtratoTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.finExtrato.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.finExtrato.Delete'),
                            confirmMessage: function (data) {
                                return l('finExtratoDeletionConfirmationMessage', data.record.id);
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
                title: l('finExtrato:idExtrato'),
                data: "idExtrato",
            },
            {
                title: l('finExtrato:idConta'),
                data: "idConta",
            },
            {
                title: l('finExtrato:idLancamento'),
                data: "idLancamento",
            },
            {
                title: l('finExtrato:transferencia'),
                data: "transferencia",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finExtrato:idExtratoRel'),
                data: "idExtratoRel",
            },
            {
                title: l('finExtrato:data'),
                data: "data",
            },
            {
                title: l('finExtrato:descricao'),
                data: "descricao",
            },
            {
                title: l('finExtrato:credito'),
                data: "credito",
            },
            {
                title: l('finExtrato:debito'),
                data: "debito",
            },
            {
                title: l('finExtrato:conferido'),
                data: "conferido",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finExtrato:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finExtrato:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('finExtrato:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('finExtrato:idUsuarioInclusao'),
                data: "idUsuarioInclusao",
            },
            {
                title: l('finExtrato:idUsuarioAlteracao'),
                data: "idUsuarioAlteracao",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewfinExtratoButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
