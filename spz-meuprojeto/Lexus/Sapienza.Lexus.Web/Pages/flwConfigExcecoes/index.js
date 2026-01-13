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

    $("#flwConfigExcecoesFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#flwConfigExcecoesFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/flwConfigExcecoesFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'flwConfigExcecoes/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'flwConfigExcecoes/EditModal');

    var dataTable = $('#flwConfigExcecoesTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.flwConfigExcecoes.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.flwConfigExcecoes.Delete'),
                            confirmMessage: function (data) {
                                return l('flwConfigExcecoesDeletionConfirmationMessage', data.record.id);
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
                title: l('flwConfigExcecoes:idConfig'),
                data: "idConfig",
            },
            {
                title: l('flwConfigExcecoes:tipoMarcacoes'),
                data: "tipoMarcacoes",
            },
            {
                title: l('flwConfigExcecoes:idHistoricoTipo'),
                data: "idHistoricoTipo",
            },
            {
                title: l('flwConfigExcecoes:data'),
                data: "data",
            },
            {
                title: l('flwConfigExcecoes:qtde'),
                data: "qtde",
            },
            {
                title: l('flwConfigExcecoes:manhaQtde'),
                data: "manhaQtde",
            },
            {
                title: l('flwConfigExcecoes:tardeQtde'),
                data: "tardeQtde",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewflwConfigExcecoesButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
