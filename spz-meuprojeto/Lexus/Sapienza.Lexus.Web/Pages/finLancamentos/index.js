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

    $("#finLancamentosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#finLancamentosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/finLancamentosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'finLancamentos/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'finLancamentos/EditModal');

    var dataTable = $('#finLancamentosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.finLancamentos.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.finLancamentos.Delete'),
                            confirmMessage: function (data) {
                                return l('finLancamentosDeletionConfirmationMessage', data.record.id);
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
                title: l('finLancamentos:idLancamento'),
                data: "idLancamento",
            },
            {
                title: l('finLancamentos:idConta'),
                data: "idConta",
            },
            {
                title: l('finLancamentos:idPlanoConta'),
                data: "idPlanoConta",
            },
            {
                title: l('finLancamentos:idCentroCusto'),
                data: "idCentroCusto",
            },
            {
                title: l('finLancamentos:operacao'),
                data: "operacao",
            },
            {
                title: l('finLancamentos:idForma'),
                data: "idForma",
            },
            {
                title: l('finLancamentos:modulo'),
                data: "modulo",
            },
            {
                title: l('finLancamentos:idCadastro'),
                data: "idCadastro",
            },
            {
                title: l('finLancamentos:idPedido'),
                data: "idPedido",
            },
            {
                title: l('finLancamentos:descricao'),
                data: "descricao",
            },
            {
                title: l('finLancamentos:nrDocumento'),
                data: "nrDocumento",
            },
            {
                title: l('finLancamentos:valor'),
                data: "valor",
            },
            {
                title: l('finLancamentos:dataEmissao'),
                data: "dataEmissao",
            },
            {
                title: l('finLancamentos:dataVencimento'),
                data: "dataVencimento",
            },
            {
                title: l('finLancamentos:dataQuitacao'),
                data: "dataQuitacao",
            },
            {
                title: l('finLancamentos:quitado'),
                data: "quitado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:recorrente'),
                data: "recorrente",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:recorrenteChave'),
                data: "recorrenteChave",
            },
            {
                title: l('finLancamentos:previsao'),
                data: "previsao",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:cobrancaEnviada'),
                data: "cobrancaEnviada",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:parcelado'),
                data: "parcelado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:identificacao'),
                data: "identificacao",
            },
            {
                title: l('finLancamentos:observacao'),
                data: "observacao",
            },
            {
                title: l('finLancamentos:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('finLancamentos:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('finLancamentos:idUsuarioInclusao'),
                data: "idUsuarioInclusao",
            },
            {
                title: l('finLancamentos:idUsuarioAlteracao'),
                data: "idUsuarioAlteracao",
            },
            {
                title: l('finLancamentos:parcela'),
                data: "parcela",
            },
            {
                title: l('finLancamentos:parcelaMaxima'),
                data: "parcelaMaxima",
            },
            {
                title: l('finLancamentos:dataVencimentoOriginal'),
                data: "dataVencimentoOriginal",
            },
            {
                title: l('finLancamentos:pagtoLiberado'),
                data: "pagtoLiberado",
            },
            {
                title: l('finLancamentos:dataParaPrevisao'),
                data: "dataParaPrevisao",
            },
            {
                title: l('finLancamentos:recorrenteVencendoVisto'),
                data: "recorrenteVencendoVisto",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:recebimentoFuturo'),
                data: "recebimentoFuturo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:recebimentoFuturoRel'),
                data: "recebimentoFuturoRel",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:idTerceiro'),
                data: "idTerceiro",
            },
            {
                title: l('finLancamentos:arquivoDocumento'),
                data: "arquivoDocumento",
            },
            {
                title: l('finLancamentos:arquivoComprovante'),
                data: "arquivoComprovante",
            },
            {
                title: l('finLancamentos:idClientePagar'),
                data: "idClientePagar",
            },
            {
                title: l('finLancamentos:idProcessoPagar'),
                data: "idProcessoPagar",
            },
            {
                title: l('finLancamentos:idArea'),
                data: "idArea",
            },
            {
                title: l('finLancamentos:identificacaoPagar'),
                data: "identificacaoPagar",
            },
            {
                title: l('finLancamentos:identificacaoPagar2'),
                data: "identificacaoPagar2",
            },
            {
                title: l('finLancamentos:arquivoDocumento2'),
                data: "arquivoDocumento2",
            },
            {
                title: l('finLancamentos:arquivoComprovante2'),
                data: "arquivoComprovante2",
            },
            {
                title: l('finLancamentos:verba'),
                data: "verba",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:verbaDataDe'),
                data: "verbaDataDe",
            },
            {
                title: l('finLancamentos:verbaDataAte'),
                data: "verbaDataAte",
            },
            {
                title: l('finLancamentos:verbaEstado'),
                data: "verbaEstado",
            },
            {
                title: l('finLancamentos:verbaCidade'),
                data: "verbaCidade",
            },
            {
                title: l('finLancamentos:idCentroResultado'),
                data: "idCentroResultado",
            },
            {
                title: l('finLancamentos:secundaria'),
                data: "secundaria",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:geradoPeloProcesso'),
                data: "geradoPeloProcesso",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:sequenciaHerdeiro'),
                data: "sequenciaHerdeiro",
            },
            {
                title: l('finLancamentos:idUnidade'),
                data: "idUnidade",
            },
            {
                title: l('finLancamentos:rateioFeito'),
                data: "rateioFeito",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:naoAbatePagtoDoSaldoDoCliente'),
                data: "naoAbatePagtoDoSaldoDoCliente",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finLancamentos:idHonorario'),
                data: "idHonorario",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewfinLancamentosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
