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

    $("#flwFollowsFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#flwFollowsFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/flwFollowsFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'flwFollows/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'flwFollows/EditModal');

    var dataTable = $('#flwFollowsTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.flwFollows.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.flwFollows.Delete'),
                            confirmMessage: function (data) {
                                return l('flwFollowsDeletionConfirmationMessage', data.record.id);
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
                title: l('flwFollows:idFollow'),
                data: "idFollow",
            },
            {
                title: l('flwFollows:idCliente'),
                data: "idCliente",
            },
            {
                title: l('flwFollows:idAcao'),
                data: "idAcao",
            },
            {
                title: l('flwFollows:idTipo'),
                data: "idTipo",
            },
            {
                title: l('flwFollows:idUsuario'),
                data: "idUsuario",
            },
            {
                title: l('flwFollows:data'),
                data: "data",
            },
            {
                title: l('flwFollows:horario'),
                data: "horario",
            },
            {
                title: l('flwFollows:comentario'),
                data: "comentario",
            },
            {
                title: l('flwFollows:finalizado'),
                data: "finalizado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('flwFollows:dataFinalizacao'),
                data: "dataFinalizacao",
            },
            {
                title: l('flwFollows:horarioFinalizacao'),
                data: "horarioFinalizacao",
            },
            {
                title: l('flwFollows:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('flwFollows:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('flwFollows:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('flwFollows:idOportunidade'),
                data: "idOportunidade",
            },
            {
                title: l('flwFollows:chegou'),
                data: "chegou",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('flwFollows:tsChegou'),
                data: "tsChegou",
                dataFormat: 'datetime'
            },
            {
                title: l('flwFollows:naoComparecimento'),
                data: "naoComparecimento",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('flwFollows:prioridade'),
                data: "prioridade",
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

    $('#NewflwFollowsButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
