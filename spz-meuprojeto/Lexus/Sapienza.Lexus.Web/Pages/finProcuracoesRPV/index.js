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

    $("#finProcuracoesRPVFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#finProcuracoesRPVFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/finProcuracoesRPVFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'finProcuracoesRPV/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'finProcuracoesRPV/EditModal');

    var dataTable = $('#finProcuracoesRPVTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.finProcuracoesRPV.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.finProcuracoesRPV.Delete'),
                            confirmMessage: function (data) {
                                return l('finProcuracoesRPVDeletionConfirmationMessage', data.record.id);
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
                title: l('finProcuracoesRPV:idProcuracao'),
                data: "idProcuracao",
            },
            {
                title: l('finProcuracoesRPV:idCliente'),
                data: "idCliente",
            },
            {
                title: l('finProcuracoesRPV:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('finProcuracoesRPV:impressa'),
                data: "impressa",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finProcuracoesRPV:tsImpressa'),
                data: "tsImpressa",
                dataFormat: 'datetime'
            },
            {
                title: l('finProcuracoesRPV:assinada'),
                data: "assinada",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finProcuracoesRPV:tsAssinatura'),
                data: "tsAssinatura",
                dataFormat: 'datetime'
            },
            {
                title: l('finProcuracoesRPV:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finProcuracoesRPV:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('finProcuracoesRPV:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewfinProcuracoesRPVButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
