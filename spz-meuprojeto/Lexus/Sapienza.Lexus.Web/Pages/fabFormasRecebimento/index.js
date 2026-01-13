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

    $("#fabFormasRecebimentoFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#fabFormasRecebimentoFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/fabFormasRecebimentoFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'fabFormasRecebimento/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'fabFormasRecebimento/EditModal');

    var dataTable = $('#fabFormasRecebimentoTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.fabFormasRecebimento.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.fabFormasRecebimento.Delete'),
                            confirmMessage: function (data) {
                                return l('fabFormasRecebimentoDeletionConfirmationMessage', data.record.id);
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
                title: l('fabFormasRecebimento:idFormaRecebimento'),
                data: "idFormaRecebimento",
            },
            {
                title: l('fabFormasRecebimento:titulo'),
                data: "titulo",
            },
            {
                title: l('fabFormasRecebimento:ordem'),
                data: "ordem",
            },
            {
                title: l('fabFormasRecebimento:padrao'),
                data: "padrao",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabFormasRecebimento:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabFormasRecebimento:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('fabFormasRecebimento:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('fabFormasRecebimento:idCondicaoPagamento'),
                data: "idCondicaoPagamento",
            },
            {
                title: l('fabFormasRecebimento:online'),
                data: "online",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabFormasRecebimento:tipo'),
                data: "tipo",
            },
            {
                title: l('fabFormasRecebimento:emailPagSeguro'),
                data: "emailPagSeguro",
            },
            {
                title: l('fabFormasRecebimento:texto'),
                data: "texto",
            },
            {
                title: l('fabFormasRecebimento:contasReceber'),
                data: "contasReceber",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabFormasRecebimento:vendas'),
                data: "vendas",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabFormasRecebimento:diasParaPrevisao'),
                data: "diasParaPrevisao",
            },
            {
                title: l('fabFormasRecebimento:valorDesconto'),
                data: "valorDesconto",
            },
            {
                title: l('fabFormasRecebimento:descontoTipo'),
                data: "descontoTipo",
            },
            {
                title: l('fabFormasRecebimento:recebimentoFuturo'),
                data: "recebimentoFuturo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabFormasRecebimento:recebimentoFuturoDias'),
                data: "recebimentoFuturoDias",
            },
            {
                title: l('fabFormasRecebimento:recebimentoFuturoTaxa'),
                data: "recebimentoFuturoTaxa",
            },
            {
                title: l('fabFormasRecebimento:idConta'),
                data: "idConta",
            },
            {
                title: l('fabFormasRecebimento:idPlanoConta'),
                data: "idPlanoConta",
            },
            {
                title: l('fabFormasRecebimento:idCentroCusto'),
                data: "idCentroCusto",
            },
            {
                title: l('fabFormasRecebimento:idContaPagar'),
                data: "idContaPagar",
            },
            {
                title: l('fabFormasRecebimento:idPlanoContaPagar'),
                data: "idPlanoContaPagar",
            },
            {
                title: l('fabFormasRecebimento:idCentroCustoPagar'),
                data: "idCentroCustoPagar",
            },
            {
                title: l('fabFormasRecebimento:idFormaPagar'),
                data: "idFormaPagar",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewfabFormasRecebimentoButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
