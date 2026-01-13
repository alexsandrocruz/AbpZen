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

    $("#logAcoesFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#logAcoesFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/logAcoesFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'logAcoes/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'logAcoes/EditModal');

    var dataTable = $('#logAcoesTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.logAcoes.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.logAcoes.Delete'),
                            confirmMessage: function (data) {
                                return l('logAcoesDeletionConfirmationMessage', data.record.id);
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
                title: l('logAcoes:idLog'),
                data: "idLog",
            },
            {
                title: l('logAcoes:area'),
                data: "area",
            },
            {
                title: l('logAcoes:acao'),
                data: "acao",
            },
            {
                title: l('logAcoes:usuario'),
                data: "usuario",
            },
            {
                title: l('logAcoes:motivo'),
                data: "motivo",
            },
            {
                title: l('logAcoes:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('logAcoes:idCliente'),
                data: "idCliente",
            },
            {
                title: l('logAcoes:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('logAcoes:idCompromisso'),
                data: "idCompromisso",
            },
            {
                title: l('logAcoes:idTarefa'),
                data: "idTarefa",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewlogAcoesButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
