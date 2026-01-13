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

    $("#advProNaturezasFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advProNaturezasFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advProNaturezasFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advProNaturezas/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advProNaturezas/EditModal');

    var dataTable = $('#advProNaturezasTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advProNaturezas.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advProNaturezas.Delete'),
                            confirmMessage: function (data) {
                                return l('advProNaturezasDeletionConfirmationMessage', data.record.id);
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
                title: l('advProNaturezas:idNatureza'),
                data: "idNatureza",
            },
            {
                title: l('advProNaturezas:titulo'),
                data: "titulo",
            },
            {
                title: l('advProNaturezas:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProNaturezas:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advProNaturezas:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advProNaturezas:mostraHistoricoNumeros'),
                data: "mostraHistoricoNumeros",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProNaturezas:recebeAcordo'),
                data: "recebeAcordo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProNaturezas:recebeRPV'),
                data: "recebeRPV",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProNaturezas:recebePrecatorio'),
                data: "recebePrecatorio",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProNaturezas:recebeAlvara'),
                data: "recebeAlvara",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProNaturezas:idArea'),
                data: "idArea",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvProNaturezasButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
