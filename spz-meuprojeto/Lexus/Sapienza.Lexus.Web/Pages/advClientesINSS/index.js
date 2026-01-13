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

    $("#advClientesINSSFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advClientesINSSFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advClientesINSSFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advClientesINSS/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advClientesINSS/EditModal');

    var dataTable = $('#advClientesINSSTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advClientesINSS.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advClientesINSS.Delete'),
                            confirmMessage: function (data) {
                                return l('advClientesINSSDeletionConfirmationMessage', data.record.id);
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
                title: l('advClientesINSS:idInssAgendado'),
                data: "idInssAgendado",
            },
            {
                title: l('advClientesINSS:idCliente'),
                data: "idCliente",
            },
            {
                title: l('advClientesINSS:inssAgendado'),
                data: "inssAgendado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientesINSS:inssData'),
                data: "inssData",
            },
            {
                title: l('advClientesINSS:inssIdTipoBeneficio'),
                data: "inssIdTipoBeneficio",
            },
            {
                title: l('advClientesINSS:inssIdPosto'),
                data: "inssIdPosto",
            },
            {
                title: l('advClientesINSS:inssResultado'),
                data: "inssResultado",
            },
            {
                title: l('advClientesINSS:tsInclusao'),
                data: "tsInclusao",
            },
            {
                title: l('advClientesINSS:tsAlteracao'),
                data: "tsAlteracao",
            },
            {
                title: l('advClientesINSS:inssResultadoIndicadorOculto'),
                data: "inssResultadoIndicadorOculto",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientesINSS:inssResponsavel'),
                data: "inssResponsavel",
            },
            {
                title: l('advClientesINSS:inssProtocolo'),
                data: "inssProtocolo",
            },
            {
                title: l('advClientesINSS:inssIdUsuarioInclusao'),
                data: "inssIdUsuarioInclusao",
            },
            {
                title: l('advClientesINSS:idStatus'),
                data: "idStatus",
            },
            {
                title: l('advClientesINSS:dataFinalizacao'),
                data: "dataFinalizacao",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvClientesINSSButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
