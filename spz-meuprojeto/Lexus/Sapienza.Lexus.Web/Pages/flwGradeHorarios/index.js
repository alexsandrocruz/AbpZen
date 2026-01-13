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

    $("#flwGradeHorariosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#flwGradeHorariosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/flwGradeHorariosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'flwGradeHorarios/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'flwGradeHorarios/EditModal');

    var dataTable = $('#flwGradeHorariosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.flwGradeHorarios.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.flwGradeHorarios.Delete'),
                            confirmMessage: function (data) {
                                return l('flwGradeHorariosDeletionConfirmationMessage', data.record.id);
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
                title: l('flwGradeHorarios:idGrade'),
                data: "idGrade",
            },
            {
                title: l('flwGradeHorarios:idHistoricoTipo'),
                data: "idHistoricoTipo",
            },
            {
                title: l('flwGradeHorarios:manhaHorarioInicial'),
                data: "manhaHorarioInicial",
            },
            {
                title: l('flwGradeHorarios:manhaIntervalo'),
                data: "manhaIntervalo",
            },
            {
                title: l('flwGradeHorarios:manhaQtde'),
                data: "manhaQtde",
            },
            {
                title: l('flwGradeHorarios:tardeHorarioInicial'),
                data: "tardeHorarioInicial",
            },
            {
                title: l('flwGradeHorarios:tardeIntervalo'),
                data: "tardeIntervalo",
            },
            {
                title: l('flwGradeHorarios:tardeQtde'),
                data: "tardeQtde",
            },
            {
                title: l('flwGradeHorarios:dom'),
                data: "dom",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('flwGradeHorarios:seg'),
                data: "seg",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('flwGradeHorarios:ter'),
                data: "ter",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('flwGradeHorarios:qua'),
                data: "qua",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('flwGradeHorarios:qui'),
                data: "qui",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('flwGradeHorarios:sex'),
                data: "sex",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('flwGradeHorarios:sab'),
                data: "sab",
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

    $('#NewflwGradeHorariosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
