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

    $("#finLancamentos_BKPFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#finLancamentos_BKPFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/finLancamentos_BKPFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'finLancamentos_BKP/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'finLancamentos_BKP/EditModal');

    var dataTable = $('#finLancamentos_BKPTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.finLancamentos_BKP.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.finLancamentos_BKP.Delete'),
                            confirmMessage: function (data) {
                                return l('finLancamentos_BKPDeletionConfirmationMessage', data.record.id);
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
                title: l('finLancamentos_BKP:idLancamento'),
                data: "idLancamento",
            },
            {
                title: l('finLancamentos_BKP:idConta'),
                data: "idConta",
            },
            {
                title: l('finLancamentos_BKP:idPlanoConta'),
                data: "idPlanoConta",
            },
            {
                title: l('finLancamentos_BKP:idCentroCusto'),
                data: "idCentroCusto",
            },
            {
                title: l('finLancamentos_BKP:operacao'),
                data: "operacao",
            },
            {
                title: l('finLancamentos_BKP:idForma'),
                data: "idForma",
            },
            {
                title: l('finLancamentos_BKP:modulo'),
                data: "modulo",
            },
            {
                title: l('finLancamentos_BKP:idCadastro'),
                data: "idCadastro",
            },
            {
                title: l('finLancamentos_BKP:idPedido'),
                data: "idPedido",
            },
            {
                title: l('finLancamentos_BKP:descricao'),
                data: "descricao",
            },
            {
                title: l('finLancamentos_BKP:nrDocumento'),
                data: "nrDocumento",
            },
            {
                title: l('finLancamentos_BKP:valor'),
                data: "valor",
            },
            {
                title: l('finLancamentos_BKP:dataEmissao'),
                data: "dataEmissao",
            },
            {
                title: l('finLancamentos_BKP:dataVencimento'),
                data: "dataVencimento",
            },
            {
                title: l('finLancamentos_BKP:dataQuitacao'),
                data: "dataQuitacao",
            },
            {
                title: l('finLancamentos_BKP:quitado'),
                data: "quitado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:recorrente'),
                data: "recorrente",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:recorrenteChave'),
                data: "recorrenteChave",
            },
            {
                title: l('finLancamentos_BKP:previsao'),
                data: "previsao",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:cobrancaEnviada'),
                data: "cobrancaEnviada",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:parcelado'),
                data: "parcelado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:identificacao'),
                data: "identificacao",
            },
            {
                title: l('finLancamentos_BKP:observacao'),
                data: "observacao",
            },
            {
                title: l('finLancamentos_BKP:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('finLancamentos_BKP:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('finLancamentos_BKP:idUsuarioInclusao'),
                data: "idUsuarioInclusao",
            },
            {
                title: l('finLancamentos_BKP:idUsuarioAlteracao'),
                data: "idUsuarioAlteracao",
            },
            {
                title: l('finLancamentos_BKP:parcela'),
                data: "parcela",
            },
            {
                title: l('finLancamentos_BKP:parcelaMaxima'),
                data: "parcelaMaxima",
            },
            {
                title: l('finLancamentos_BKP:dataVencimentoOriginal'),
                data: "dataVencimentoOriginal",
            },
            {
                title: l('finLancamentos_BKP:pagtoLiberado'),
                data: "pagtoLiberado",
            },
            {
                title: l('finLancamentos_BKP:dataParaPrevisao'),
                data: "dataParaPrevisao",
            },
            {
                title: l('finLancamentos_BKP:recorrenteVencendoVisto'),
                data: "recorrenteVencendoVisto",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:recebimentoFuturo'),
                data: "recebimentoFuturo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:recebimentoFuturoRel'),
                data: "recebimentoFuturoRel",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:idTerceiro'),
                data: "idTerceiro",
            },
            {
                title: l('finLancamentos_BKP:arquivoDocumento'),
                data: "arquivoDocumento",
            },
            {
                title: l('finLancamentos_BKP:arquivoComprovante'),
                data: "arquivoComprovante",
            },
            {
                title: l('finLancamentos_BKP:idClientePagar'),
                data: "idClientePagar",
            },
            {
                title: l('finLancamentos_BKP:idProcessoPagar'),
                data: "idProcessoPagar",
            },
            {
                title: l('finLancamentos_BKP:idArea'),
                data: "idArea",
            },
            {
                title: l('finLancamentos_BKP:identificacaoPagar'),
                data: "identificacaoPagar",
            },
            {
                title: l('finLancamentos_BKP:identificacaoPagar2'),
                data: "identificacaoPagar2",
            },
            {
                title: l('finLancamentos_BKP:arquivoDocumento2'),
                data: "arquivoDocumento2",
            },
            {
                title: l('finLancamentos_BKP:arquivoComprovante2'),
                data: "arquivoComprovante2",
            },
            {
                title: l('finLancamentos_BKP:verba'),
                data: "verba",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:verbaDataDe'),
                data: "verbaDataDe",
            },
            {
                title: l('finLancamentos_BKP:verbaDataAte'),
                data: "verbaDataAte",
            },
            {
                title: l('finLancamentos_BKP:verbaEstado'),
                data: "verbaEstado",
            },
            {
                title: l('finLancamentos_BKP:verbaCidade'),
                data: "verbaCidade",
            },
            {
                title: l('finLancamentos_BKP:idCentroResultado'),
                data: "idCentroResultado",
            },
            {
                title: l('finLancamentos_BKP:secundaria'),
                data: "secundaria",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:geradoPeloProcesso'),
                data: "geradoPeloProcesso",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos_BKP:sequenciaHerdeiro'),
                data: "sequenciaHerdeiro",
            },
            {
                title: l('finLancamentos_BKP:idUnidade'),
                data: "idUnidade",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewfinLancamentos_BKPButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
