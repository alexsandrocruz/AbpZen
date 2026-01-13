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

    $("#advCompromissosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advCompromissosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advCompromissosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advCompromissos/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advCompromissos/EditModal');

    var dataTable = $('#advCompromissosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advCompromissos.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advCompromissos.Delete'),
                            confirmMessage: function (data) {
                                return l('advCompromissosDeletionConfirmationMessage', data.record.id);
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
                title: l('advCompromissos:idCompromisso'),
                data: "idCompromisso",
            },
            {
                title: l('advCompromissos:idTipoCompromisso'),
                data: "idTipoCompromisso",
            },
            {
                title: l('advCompromissos:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('advCompromissos:dataPublicacao'),
                data: "dataPublicacao",
            },
            {
                title: l('advCompromissos:dataPrazoInterno'),
                data: "dataPrazoInterno",
            },
            {
                title: l('advCompromissos:dataPrazoFatal'),
                data: "dataPrazoFatal",
            },
            {
                title: l('advCompromissos:descricao'),
                data: "descricao",
            },
            {
                title: l('advCompromissos:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advCompromissos:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advCompromissos:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advCompromissos:incluidoPor'),
                data: "incluidoPor",
            },
            {
                title: l('advCompromissos:alteradoPor'),
                data: "alteradoPor",
            },
            {
                title: l('advCompromissos:idAgendamentoINSS'),
                data: "idAgendamentoINSS",
            },
            {
                title: l('advCompromissos:pauta'),
                data: "pauta",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advCompromissos:pautaIdUsuarioResp'),
                data: "pautaIdUsuarioResp",
            },
            {
                title: l('advCompromissos:pautaRespAceite'),
                data: "pautaRespAceite",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advCompromissos:horarioInicial'),
                data: "horarioInicial",
            },
            {
                title: l('advCompromissos:horarioFinal'),
                data: "horarioFinal",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvCompromissosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
