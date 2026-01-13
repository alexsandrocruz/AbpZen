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

    $("#finRateiosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#finRateiosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/finRateiosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'finRateios/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'finRateios/EditModal');

    var dataTable = $('#finRateiosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.finRateios.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.finRateios.Delete'),
                            confirmMessage: function (data) {
                                return l('finRateiosDeletionConfirmationMessage', data.record.id);
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
                title: l('finRateios:idRateio'),
                data: "idRateio",
            },
            {
                title: l('finRateios:idLancamento'),
                data: "idLancamento",
            },
            {
                title: l('finRateios:idCentroCusto'),
                data: "idCentroCusto",
            },
            {
                title: l('finRateios:idCentroResultado'),
                data: "idCentroResultado",
            },
            {
                title: l('finRateios:percentualCC'),
                data: "percentualCC",
            },
            {
                title: l('finRateios:percentualCR'),
                data: "percentualCR",
            },
            {
                title: l('finRateios:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('finRateios:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('finRateios:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('finRateios:idUnidade'),
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

    $('#NewfinRateiosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
