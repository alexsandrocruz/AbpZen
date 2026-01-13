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

    $("#opoOrcamentosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#opoOrcamentosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/opoOrcamentosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'opoOrcamentos/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'opoOrcamentos/EditModal');

    var dataTable = $('#opoOrcamentosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.opoOrcamentos.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.opoOrcamentos.Delete'),
                            confirmMessage: function (data) {
                                return l('opoOrcamentosDeletionConfirmationMessage', data.record.id);
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
                title: l('opoOrcamentos:idOrcamento'),
                data: "idOrcamento",
            },
            {
                title: l('opoOrcamentos:idOportunidade'),
                data: "idOportunidade",
            },
            {
                title: l('opoOrcamentos:titulo'),
                data: "titulo",
            },
            {
                title: l('opoOrcamentos:dataCriacao'),
                data: "dataCriacao",
            },
            {
                title: l('opoOrcamentos:valor'),
                data: "valor",
            },
            {
                title: l('opoOrcamentos:arquivo'),
                data: "arquivo",
            },
            {
                title: l('opoOrcamentos:aceito'),
                data: "aceito",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOrcamentos:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOrcamentos:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('opoOrcamentos:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('opoOrcamentos:dataValidade'),
                data: "dataValidade",
            },
            {
                title: l('opoOrcamentos:valorMensal'),
                data: "valorMensal",
            },
            {
                title: l('opoOrcamentos:comArquivo'),
                data: "comArquivo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOrcamentos:comProduto'),
                data: "comProduto",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOrcamentos:comProdutoTerceiro'),
                data: "comProdutoTerceiro",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOrcamentos:comServico'),
                data: "comServico",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('opoOrcamentos:valorDesconto'),
                data: "valorDesconto",
            },
            {
                title: l('opoOrcamentos:valorAcrescimo'),
                data: "valorAcrescimo",
            },
            {
                title: l('opoOrcamentos:valorFrete'),
                data: "valorFrete",
            },
            {
                title: l('opoOrcamentos:informacoes'),
                data: "informacoes",
            },
            {
                title: l('opoOrcamentos:descontoPercentual'),
                data: "descontoPercentual",
            },
            {
                title: l('opoOrcamentos:valorItens'),
                data: "valorItens",
            },
            {
                title: l('opoOrcamentos:idCondicaoPagamento'),
                data: "idCondicaoPagamento",
            },
            {
                title: l('opoOrcamentos:dataPrevistaEntrega'),
                data: "dataPrevistaEntrega",
            },
            {
                title: l('opoOrcamentos:moeda'),
                data: "moeda",
            },
            {
                title: l('opoOrcamentos:valorConversao'),
                data: "valorConversao",
            },
            {
                title: l('opoOrcamentos:imprimeMoedaAdd'),
                data: "imprimeMoedaAdd",
            },
            {
                title: l('opoOrcamentos:valorDescontoMensal'),
                data: "valorDescontoMensal",
            },
            {
                title: l('opoOrcamentos:valorAcrescimoMensal'),
                data: "valorAcrescimoMensal",
            },
            {
                title: l('opoOrcamentos:valorFreteMensal'),
                data: "valorFreteMensal",
            },
            {
                title: l('opoOrcamentos:descontoPercentualMensal'),
                data: "descontoPercentualMensal",
            },
            {
                title: l('opoOrcamentos:valorItensMensal'),
                data: "valorItensMensal",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewopoOrcamentosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
