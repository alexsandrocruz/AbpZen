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

    $("#advVerbasFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advVerbasFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advVerbasFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advVerbas/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advVerbas/EditModal');

    var dataTable = $('#advVerbasTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advVerbas.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advVerbas.Delete'),
                            confirmMessage: function (data) {
                                return l('advVerbasDeletionConfirmationMessage', data.record.id);
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
                title: l('advVerbas:idVerba'),
                data: "idVerba",
            },
            {
                title: l('advVerbas:idTipo'),
                data: "idTipo",
            },
            {
                title: l('advVerbas:idProfissional'),
                data: "idProfissional",
            },
            {
                title: l('advVerbas:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('advVerbas:idLancamento'),
                data: "idLancamento",
            },
            {
                title: l('advVerbas:valor'),
                data: "valor",
            },
            {
                title: l('advVerbas:dataDe'),
                data: "dataDe",
            },
            {
                title: l('advVerbas:dataAte'),
                data: "dataAte",
            },
            {
                title: l('advVerbas:estado'),
                data: "estado",
            },
            {
                title: l('advVerbas:cidade'),
                data: "cidade",
            },
            {
                title: l('advVerbas:comprovante'),
                data: "comprovante",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advVerbas:comprovanteArquivo'),
                data: "comprovanteArquivo",
            },
            {
                title: l('advVerbas:solicitacao'),
                data: "solicitacao",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advVerbas:aceito'),
                data: "aceito",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advVerbas:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advVerbas:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advVerbas:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advVerbas:incluidoPor'),
                data: "incluidoPor",
            },
            {
                title: l('advVerbas:alteradoPor'),
                data: "alteradoPor",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvVerbasButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
