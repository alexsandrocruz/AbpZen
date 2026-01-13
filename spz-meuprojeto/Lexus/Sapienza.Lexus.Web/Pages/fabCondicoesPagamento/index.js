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

    $("#fabCondicoesPagamentoFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#fabCondicoesPagamentoFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/fabCondicoesPagamentoFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'fabCondicoesPagamento/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'fabCondicoesPagamento/EditModal');

    var dataTable = $('#fabCondicoesPagamentoTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.fabCondicoesPagamento.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.fabCondicoesPagamento.Delete'),
                            confirmMessage: function (data) {
                                return l('fabCondicoesPagamentoDeletionConfirmationMessage', data.record.id);
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
                title: l('fabCondicoesPagamento:idCondicaoPagamento'),
                data: "idCondicaoPagamento",
            },
            {
                title: l('fabCondicoesPagamento:titulo'),
                data: "titulo",
            },
            {
                title: l('fabCondicoesPagamento:parcelas'),
                data: "parcelas",
            },
            {
                title: l('fabCondicoesPagamento:p1'),
                data: "p1",
            },
            {
                title: l('fabCondicoesPagamento:d1'),
                data: "d1",
            },
            {
                title: l('fabCondicoesPagamento:p2'),
                data: "p2",
            },
            {
                title: l('fabCondicoesPagamento:d2'),
                data: "d2",
            },
            {
                title: l('fabCondicoesPagamento:p3'),
                data: "p3",
            },
            {
                title: l('fabCondicoesPagamento:d3'),
                data: "d3",
            },
            {
                title: l('fabCondicoesPagamento:p4'),
                data: "p4",
            },
            {
                title: l('fabCondicoesPagamento:d4'),
                data: "d4",
            },
            {
                title: l('fabCondicoesPagamento:p5'),
                data: "p5",
            },
            {
                title: l('fabCondicoesPagamento:d5'),
                data: "d5",
            },
            {
                title: l('fabCondicoesPagamento:p6'),
                data: "p6",
            },
            {
                title: l('fabCondicoesPagamento:d6'),
                data: "d6",
            },
            {
                title: l('fabCondicoesPagamento:p7'),
                data: "p7",
            },
            {
                title: l('fabCondicoesPagamento:d7'),
                data: "d7",
            },
            {
                title: l('fabCondicoesPagamento:p8'),
                data: "p8",
            },
            {
                title: l('fabCondicoesPagamento:d8'),
                data: "d8",
            },
            {
                title: l('fabCondicoesPagamento:p9'),
                data: "p9",
            },
            {
                title: l('fabCondicoesPagamento:d9'),
                data: "d9",
            },
            {
                title: l('fabCondicoesPagamento:p10'),
                data: "p10",
            },
            {
                title: l('fabCondicoesPagamento:d10'),
                data: "d10",
            },
            {
                title: l('fabCondicoesPagamento:p11'),
                data: "p11",
            },
            {
                title: l('fabCondicoesPagamento:d11'),
                data: "d11",
            },
            {
                title: l('fabCondicoesPagamento:p12'),
                data: "p12",
            },
            {
                title: l('fabCondicoesPagamento:d12'),
                data: "d12",
            },
            {
                title: l('fabCondicoesPagamento:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabCondicoesPagamento:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('fabCondicoesPagamento:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('fabCondicoesPagamento:compras'),
                data: "compras",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabCondicoesPagamento:vendas'),
                data: "vendas",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabCondicoesPagamento:valorMinimo'),
                data: "valorMinimo",
            },
            {
                title: l('fabCondicoesPagamento:atendimento'),
                data: "atendimento",
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

    $('#NewfabCondicoesPagamentoButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
