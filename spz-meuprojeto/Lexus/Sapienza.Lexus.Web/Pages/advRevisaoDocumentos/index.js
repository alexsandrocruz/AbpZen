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

    $("#advRevisaoDocumentosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advRevisaoDocumentosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advRevisaoDocumentosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advRevisaoDocumentos/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advRevisaoDocumentos/EditModal');

    var dataTable = $('#advRevisaoDocumentosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advRevisaoDocumentos.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advRevisaoDocumentos.Delete'),
                            confirmMessage: function (data) {
                                return l('advRevisaoDocumentosDeletionConfirmationMessage', data.record.id);
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
                title: l('advRevisaoDocumentos:idRevisao'),
                data: "idRevisao",
            },
            {
                title: l('advRevisaoDocumentos:idUsuarioSolicitante'),
                data: "idUsuarioSolicitante",
            },
            {
                title: l('advRevisaoDocumentos:idUsuarioRevisor'),
                data: "idUsuarioRevisor",
            },
            {
                title: l('advRevisaoDocumentos:localRede'),
                data: "localRede",
            },
            {
                title: l('advRevisaoDocumentos:pendente'),
                data: "pendente",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advRevisaoDocumentos:aprovado'),
                data: "aprovado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advRevisaoDocumentos:reprovado'),
                data: "reprovado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advRevisaoDocumentos:finalizado'),
                data: "finalizado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advRevisaoDocumentos:comentariosSolicitante'),
                data: "comentariosSolicitante",
            },
            {
                title: l('advRevisaoDocumentos:comentariosRevisor'),
                data: "comentariosRevisor",
            },
            {
                title: l('advRevisaoDocumentos:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advRevisaoDocumentos:incluidoPor'),
                data: "incluidoPor",
            },
            {
                title: l('advRevisaoDocumentos:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advRevisaoDocumentos:alteradoPor'),
                data: "alteradoPor",
            },
            {
                title: l('advRevisaoDocumentos:ativo'),
                data: "ativo",
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

    $('#NewadvRevisaoDocumentosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
