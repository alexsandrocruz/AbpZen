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

    $("#fabDatasEFeriadosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#fabDatasEFeriadosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/fabDatasEFeriadosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'fabDatasEFeriados/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'fabDatasEFeriados/EditModal');

    var dataTable = $('#fabDatasEFeriadosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.fabDatasEFeriados.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.fabDatasEFeriados.Delete'),
                            confirmMessage: function (data) {
                                return l('fabDatasEFeriadosDeletionConfirmationMessage', data.record.id);
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
                title: l('fabDatasEFeriados:idData'),
                data: "idData",
            },
            {
                title: l('fabDatasEFeriados:titulo'),
                data: "titulo",
            },
            {
                title: l('fabDatasEFeriados:data'),
                data: "data",
            },
            {
                title: l('fabDatasEFeriados:feriado'),
                data: "feriado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabDatasEFeriados:fixo'),
                data: "fixo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabDatasEFeriados:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('fabDatasEFeriados:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('fabDatasEFeriados:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewfabDatasEFeriadosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
