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

    $("#finPlanoContasFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#finPlanoContasFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/finPlanoContasFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'finPlanoContas/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'finPlanoContas/EditModal');

    var dataTable = $('#finPlanoContasTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.finPlanoContas.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.finPlanoContas.Delete'),
                            confirmMessage: function (data) {
                                return l('finPlanoContasDeletionConfirmationMessage', data.record.id);
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
                title: l('finPlanoContas:idPlanoConta'),
                data: "idPlanoConta",
            },
            {
                title: l('finPlanoContas:idGrupo'),
                data: "idGrupo",
            },
            {
                title: l('finPlanoContas:titulo'),
                data: "titulo",
            },
            {
                title: l('finPlanoContas:codigo'),
                data: "codigo",
            },
            {
                title: l('finPlanoContas:pagamentoSempreLiberado'),
                data: "pagamentoSempreLiberado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finPlanoContas:permiteLancamentoQuitado'),
                data: "permiteLancamentoQuitado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finPlanoContas:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finPlanoContas:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('finPlanoContas:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('finPlanoContas:padraoVendas'),
                data: "padraoVendas",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finPlanoContas:antecipaVencimento'),
                data: "antecipaVencimento",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finPlanoContas:terceiroNivel'),
                data: "terceiroNivel",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finPlanoContas:criarPeloFinanceiro'),
                data: "criarPeloFinanceiro",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finPlanoContas:valoresRestritos'),
                data: "valoresRestritos",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finPlanoContas:naoAbatePagtoDoSaldoDoCliente'),
                data: "naoAbatePagtoDoSaldoDoCliente",
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

    $('#NewfinPlanoContasButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
