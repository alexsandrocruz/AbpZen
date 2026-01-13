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

    $("#advClientesHistoricosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advClientesHistoricosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advClientesHistoricosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advClientesHistoricos/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advClientesHistoricos/EditModal');

    var dataTable = $('#advClientesHistoricosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advClientesHistoricos.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advClientesHistoricos.Delete'),
                            confirmMessage: function (data) {
                                return l('advClientesHistoricosDeletionConfirmationMessage', data.record.id);
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
                title: l('advClientesHistoricos:idHistorico'),
                data: "idHistorico",
            },
            {
                title: l('advClientesHistoricos:idCliente'),
                data: "idCliente",
            },
            {
                title: l('advClientesHistoricos:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('advClientesHistoricos:idUsuario'),
                data: "idUsuario",
            },
            {
                title: l('advClientesHistoricos:idTipoHistorico'),
                data: "idTipoHistorico",
            },
            {
                title: l('advClientesHistoricos:data'),
                data: "data",
            },
            {
                title: l('advClientesHistoricos:hora'),
                data: "hora",
            },
            {
                title: l('advClientesHistoricos:ocorrencia'),
                data: "ocorrencia",
            },
            {
                title: l('advClientesHistoricos:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advClientesHistoricos:idOportunidade'),
                data: "idOportunidade",
            },
            {
                title: l('advClientesHistoricos:depto'),
                data: "depto",
            },
            {
                title: l('advClientesHistoricos:prioritario'),
                data: "prioritario",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvClientesHistoricosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
