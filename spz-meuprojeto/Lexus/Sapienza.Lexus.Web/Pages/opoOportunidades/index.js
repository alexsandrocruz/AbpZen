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

    $("#opoOportunidadesFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#opoOportunidadesFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/opoOportunidadesFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'opoOportunidades/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'opoOportunidades/EditModal');

    var dataTable = $('#opoOportunidadesTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.opoOportunidades.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.opoOportunidades.Delete'),
                            confirmMessage: function (data) {
                                return l('opoOportunidadesDeletionConfirmationMessage', data.record.id);
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
                title: l('opoOportunidades:idOportunidade'),
                data: "idOportunidade",
            },
            {
                title: l('opoOportunidades:idCliente'),
                data: "idCliente",
            },
            {
                title: l('opoOportunidades:idUsuario'),
                data: "idUsuario",
            },
            {
                title: l('opoOportunidades:idTipo'),
                data: "idTipo",
            },
            {
                title: l('opoOportunidades:idSituacao'),
                data: "idSituacao",
            },
            {
                title: l('opoOportunidades:titulo'),
                data: "titulo",
            },
            {
                title: l('opoOportunidades:numero'),
                data: "numero",
            },
            {
                title: l('opoOportunidades:dataInicio'),
                data: "dataInicio",
            },
            {
                title: l('opoOportunidades:dataEstimada'),
                data: "dataEstimada",
            },
            {
                title: l('opoOportunidades:valorEstimado'),
                data: "valorEstimado",
            },
            {
                title: l('opoOportunidades:comentario'),
                data: "comentario",
            },
            {
                title: l('opoOportunidades:aproveitada'),
                data: "aproveitada",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOportunidades:aproveitadaData'),
                data: "aproveitadaData",
            },
            {
                title: l('opoOportunidades:cancelada'),
                data: "cancelada",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOportunidades:canceladaMotivo'),
                data: "canceladaMotivo",
            },
            {
                title: l('opoOportunidades:canceladaData'),
                data: "canceladaData",
            },
            {
                title: l('opoOportunidades:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOportunidades:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('opoOportunidades:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('opoOportunidades:indicadorCanceladoVisto'),
                data: "indicadorCanceladoVisto",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOportunidades:valorEstimadoMensal'),
                data: "valorEstimadoMensal",
            },
            {
                title: l('opoOportunidades:deProcesso'),
                data: "deProcesso",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOportunidades:aproveitadaMotivo'),
                data: "aproveitadaMotivo",
            },
            {
                title: l('opoOportunidades:numeroProcesso'),
                data: "numeroProcesso",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewopoOportunidadesButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
