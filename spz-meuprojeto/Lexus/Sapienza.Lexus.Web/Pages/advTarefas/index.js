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

    $("#advTarefasFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advTarefasFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advTarefasFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advTarefas/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advTarefas/EditModal');

    var dataTable = $('#advTarefasTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advTarefas.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advTarefas.Delete'),
                            confirmMessage: function (data) {
                                return l('advTarefasDeletionConfirmationMessage', data.record.id);
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
                title: l('advTarefas:idTarefa'),
                data: "idTarefa",
            },
            {
                title: l('advTarefas:idTipoTarefa'),
                data: "idTipoTarefa",
            },
            {
                title: l('advTarefas:idCompromisso'),
                data: "idCompromisso",
            },
            {
                title: l('advTarefas:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('advTarefas:dataCadastro'),
                data: "dataCadastro",
            },
            {
                title: l('advTarefas:dataParaFinalizacao'),
                data: "dataParaFinalizacao",
            },
            {
                title: l('advTarefas:descricao'),
                data: "descricao",
            },
            {
                title: l('advTarefas:idResponsavel'),
                data: "idResponsavel",
            },
            {
                title: l('advTarefas:idExecutor'),
                data: "idExecutor",
            },
            {
                title: l('advTarefas:finalizado'),
                data: "finalizado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advTarefas:tsFinalizacao'),
                data: "tsFinalizacao",
                dataFormat: 'datetime'
            },
            {
                title: l('advTarefas:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advTarefas:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advTarefas:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advTarefas:incluidoPor'),
                data: "incluidoPor",
            },
            {
                title: l('advTarefas:alteradoPor'),
                data: "alteradoPor",
            },
            {
                title: l('advTarefas:agendada'),
                data: "agendada",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advTarefas:horarioInicial'),
                data: "horarioInicial",
            },
            {
                title: l('advTarefas:horarioFinal'),
                data: "horarioFinal",
            },
            {
                title: l('advTarefas:onde'),
                data: "onde",
            },
            {
                title: l('advTarefas:idCliente'),
                data: "idCliente",
            },
            {
                title: l('advTarefas:idUsuarioFinalizou'),
                data: "idUsuarioFinalizou",
            },
            {
                title: l('advTarefas:lembreteQuandoFinalizarPara'),
                data: "lembreteQuandoFinalizarPara",
            },
            {
                title: l('advTarefas:tecnica'),
                data: "tecnica",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advTarefas:coletivoOriginal'),
                data: "coletivoOriginal",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advTarefas:coletivoIdOriginal'),
                data: "coletivoIdOriginal",
            },
            {
                title: l('advTarefas:coletivoIdCliente'),
                data: "coletivoIdCliente",
            },
            {
                title: l('advTarefas:pauta'),
                data: "pauta",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advTarefas:pautaIdUsuarioResp'),
                data: "pautaIdUsuarioResp",
            },
            {
                title: l('advTarefas:pautaRespAceite'),
                data: "pautaRespAceite",
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

    $('#NewadvTarefasButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
