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

    $("#usuDistanciasFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#usuDistanciasFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/usuDistanciasFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'usuDistancias/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'usuDistancias/EditModal');

    var dataTable = $('#usuDistanciasTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.usuDistancias.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.usuDistancias.Delete'),
                            confirmMessage: function (data) {
                                return l('usuDistanciasDeletionConfirmationMessage', data.record.id);
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
                title: l('usuDistancias:idDistancia'),
                data: "idDistancia",
            },
            {
                title: l('usuDistancias:idUsuario'),
                data: "idUsuario",
            },
            {
                title: l('usuDistancias:estado'),
                data: "estado",
            },
            {
                title: l('usuDistancias:cidade'),
                data: "cidade",
            },
            {
                title: l('usuDistancias:km'),
                data: "km",
            },
            {
                title: l('usuDistancias:tsInclusao'),
                data: "tsInclusao",
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

    $('#NewusuDistanciasButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
