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

    $("#advClientesArquivosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advClientesArquivosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advClientesArquivosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advClientesArquivos/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advClientesArquivos/EditModal');

    var dataTable = $('#advClientesArquivosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advClientesArquivos.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advClientesArquivos.Delete'),
                            confirmMessage: function (data) {
                                return l('advClientesArquivosDeletionConfirmationMessage', data.record.id);
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
                title: l('advClientesArquivos:idArquivo'),
                data: "idArquivo",
            },
            {
                title: l('advClientesArquivos:idCliente'),
                data: "idCliente",
            },
            {
                title: l('advClientesArquivos:idTipoArquivo'),
                data: "idTipoArquivo",
            },
            {
                title: l('advClientesArquivos:descricao'),
                data: "descricao",
            },
            {
                title: l('advClientesArquivos:arquivo'),
                data: "arquivo",
            },
            {
                title: l('advClientesArquivos:incluidoPor'),
                data: "incluidoPor",
            },
            {
                title: l('advClientesArquivos:alteradoPor'),
                data: "alteradoPor",
            },
            {
                title: l('advClientesArquivos:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientesArquivos:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advClientesArquivos:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advClientesArquivos:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('advClientesArquivos:precisaRevisao'),
                data: "precisaRevisao",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientesArquivos:idSolicitante'),
                data: "idSolicitante",
            },
            {
                title: l('advClientesArquivos:solicitanteComentario'),
                data: "solicitanteComentario",
            },
            {
                title: l('advClientesArquivos:idRevisor'),
                data: "idRevisor",
            },
            {
                title: l('advClientesArquivos:revisorComentario'),
                data: "revisorComentario",
            },
            {
                title: l('advClientesArquivos:reprovado'),
                data: "reprovado",
            },
            {
                title: l('advClientesArquivos:pendenteVisualizacaoAprovacao'),
                data: "pendenteVisualizacaoAprovacao",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientesArquivos:status'),
                data: "status",
            },
            {
                title: l('advClientesArquivos:autoFTP'),
                data: "autoFTP",
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

    $('#NewadvClientesArquivosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
